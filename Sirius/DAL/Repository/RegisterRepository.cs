using Microsoft.EntityFrameworkCore;
using Sirius.DAL;
using Sirius.Helpers;
using Sirius.Extends.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirius.DAL.Repository.Contract;
using System.Linq.Expressions;
using Sirius.Models.Dtos;

namespace Sirius.Models
{
    public class RegisterRepository : GenericRepository<Register>, IRegisterRepository
    {
        public RegisterRepository(SiriusContext _siriusContext) : base(_siriusContext)
        { }

        /// <summary>
        /// Получение регистров по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(
        Expression<Func<Register, bool>> filter = null,
        Func<IQueryable<Register>, IOrderedQueryable<Register>> orderBy = null,
        string includeProperties = "")
        {
            var registers = _siriusContext.Registers
               .Include(i => i.Invoice)
               .Include(i => i.Item)
               .Where(filter)
               .Select(r => new
               {
                   r.Id,
                   r.Item.Name,
                   Dimension = r.Item.Dimension.Name,
                   r.Amount,
                   r.Cost,
                   Sum = ((double)r.Cost * r.Amount),
                   Date = DateConverter.ConvertToStandardString(r.Invoice.Date),
                   r.InvoiceId
               });
            return registers;
        }

        /// <summary>
        /// Получение регистров по идентификатору накладной
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public IEnumerable<Register> GetByInvoiceId(Guid invoiceId)
        {
            return _siriusContext.Registers.Where(r => r.InvoiceId == invoiceId);
        }

        /// <summary>
        /// Получение регистров по алиасу типа накладной
        /// </summary>
        /// <param name="typeAlias"></param>
        /// <returns></returns>
        public IEnumerable<object> GetByTypeAlias(string typeAlias)
        {
            var invoiceType = _siriusContext.InvoiceTypes
                .Where(it => it.Alias == typeAlias)?
                .FirstOrDefault();

            if (invoiceType != null)
            {
                var invoices = _siriusContext.Invoices
                    .Where(i => i.TypeId == invoiceType.Id);

                var registers = invoices
                    .SelectMany(i => i.Registers)
                    .Include(r => r.Item)
                    .ThenInclude(i => i.Dimension)
                    .Select(r => new
                    {
                        r.Id,
                        r.Item.Name,
                        Dimension = r.Item.Dimension.Name,
                        r.Amount,
                        r.Cost,
                        Sum = ((double)r.Cost * r.Amount),
                        Date = DateConverter.ConvertToStandardString(r.Invoice.Date),
                        r.InvoiceId
                    });
                return registers;
            }
            return null;
        }

        /// <summary>
        /// Получение остатков по критериям фильтрации
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Batch>> GetDynamicBatchesByFilter(BatchFilter filter)
        {
            List<Batch> result = new List<Batch>();

            var t1 = Guid.Empty;
            var t2 = DateTime.MinValue;

            // Получаем регистры прихода
            var registers = await _siriusContext.Registers
                .Include(r => r.Item)
                .Include(r => r.Invoice)
                .Where(r =>
                    (r.ItemId == filter.ItemId) &&
                    (r.Invoice.IsFixed == true) &&
                    (r.Invoice.Factor > 0) &&
                    (filter.CategoryId != Guid.Empty ? r.Item.CategoryId == filter.CategoryId : true) &&
                    (filter.VendorId != Guid.Empty ? r.Invoice.VendorId == filter.VendorId : true))
                .OrderBy(r => r.Invoice.Date)
                .ToListAsync();

            List<Batch> batches = new List<Batch>();

            registers.ForEach(reg1 =>
            {
                decimal cost = reg1.Cost;
                double amount = reg1.Amount;
                registers.ForEach(reg2 =>
                {
                    if (reg1.Id != reg2.Id && reg1.Cost == reg2.Cost)
                    {
                        amount += reg2.Amount;
                    }
                });
                batches.Add(new Batch()
                {
                    Amount = Math.Round(amount, 2),
                    Cost = cost
                });
            });

            IEnumerable<Batch> arrivalBatch = batches.Distinct();

            // Получаем регистры расхода

            batches = new List<Batch>();

            registers = await _siriusContext.Registers
                .Include(r => r.Item)
                .Include(r => r.Invoice)
                .Where(r =>
                    (r.ItemId == filter.ItemId) &&
                    (r.Invoice.IsFixed == true) &&
                    (r.Invoice.Factor < 0) &&
                    (r.Item.IsCountless == false) &&
                    (filter.CategoryId != Guid.Empty ? r.Item.CategoryId == filter.CategoryId : true) &&
                    (filter.VendorId != Guid.Empty ? r.Invoice.VendorId == filter.VendorId : true))
                .OrderBy(r => r.Invoice.Date)
                .ToListAsync();

            registers.ForEach(reg1 =>
            {
                decimal cost = reg1.Cost;
                double amount = reg1.Amount;
                registers.ForEach(reg2 =>
                {
                    if (reg1.Id != reg2.Id && reg1.Cost == reg2.Cost)
                    {
                        amount += reg2.Amount;
                    }
                });
                batches.Add(new Batch()
                {
                    Amount = Math.Round(amount, 2),
                    Cost = cost
                });
            });

            IEnumerable<Batch> writeOffBatch = batches.Distinct();

            // Вычисляем остатки путём вычитания расходных значений из приходных
            arrivalBatch.ToList().ForEach(ab =>
            {
                writeOffBatch.ToList().ForEach(wb =>
                {
                    if (ab.Cost == wb.Cost)
                    {
                        ab.Amount = ab.Amount - wb.Amount;
                    }
                });
                if (ab.Amount != 0)
                {
                    result.Add(ab);
                }
            });
            // Убираем совпадения, фильтруем по цене
            result = result.Distinct().Where(b => (filter.Cost != 0 ? b.Cost == filter.Cost : true)).ToList();
            return result;
        }

        public IEnumerable<Batch> GetStaticBatchesByFilter(BatchFilter filter)
        {
            var result = _siriusContext.StorageRegisters
                .Include(r => r.Item)
                .Where(r =>
                    (r.ItemId == filter.ItemId) &&
                    (filter.CategoryId != Guid.Empty ? r.Item.CategoryId == filter.CategoryId : true) &&
                    // Если в фильтре указана выборка по критическим остаткам
                    (filter.isMinimumLimit ? r.Amount <= _siriusContext.Items.FirstOrDefault(x => x.Id == r.ItemId).MinimumLimit : true))
                .OrderBy(r => r.Item.CategoryId)
                .Select(sr => new Batch { Amount = sr.Amount, Cost = sr.Cost })
                .ToList();

            return result.Select(b => new Batch { Amount = Math.Round(b.Amount, 2), Cost = b.Cost });
        }

        /// <summary>
        /// Получение сгруппированного по наименованиям списка остатков 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<BatchGroup> GetBatches(BatchFilter filter)
        {
            List<BatchGroup> batchGroups = new List<BatchGroup>();
            Guid itemId = filter.ItemId;

            // Получаем все ids всех наименований
            var itemList = _siriusContext.Items.Select(i => new { i.Id, i.Name });

            // Если в фильтре указан itemId, то производим отбор 
            var items = itemId != Guid.Empty ? itemList.Where(i => i.Id == filter.ItemId).ToList() : itemList.ToList();

            // Получаем остатки по каждому наименованию
            items.ForEach(i =>
            {
                filter.ItemId = itemId != Guid.Empty ? filter.ItemId : i.Id;

                /** Получаем остатки в зависимости от значения параметра фильтра IsDynamic
                 * Если значение false, то ищем в накопительном регистре StorageRegiter
                 * При значении true высчитываем остатки по всем существующим регистрам проведённых накладных (возможно будет медленно)    */
                BatchGroup batchGroup = new BatchGroup()
                {
                    Name = i.Name,
                    Batches = filter.IsDynamic == true ? GetDynamicBatchesByFilter(filter).Result : GetStaticBatchesByFilter(filter)
                };
                batchGroups.Add(batchGroup);
            });

            return batchGroups;
        }

        public IEnumerable<Register> GetFixedRegisters()
        {
            return _siriusContext.Registers
                .Include(i => i.Invoice)
                .Where(x => x.Invoice.IsFixed == true);
        }

        public IEnumerable<ReportItemDto> GetReportItems()
        {
            var incoming = _siriusContext.Registers
                .Include(i => i.Invoice)
                .Include(i => i.Item)
                .Where(i => i.Invoice.TypeId == Types.InvoiceTypes.Arrival.Id).Select(i => new ReportItem()
                {
                    Id = i.ItemId,
                    Name = i.Item.Name,
                    Amount = i.Amount,
                }).ToList();

            var consumption = _siriusContext.Registers
                .Include(i => i.Invoice)
                .Include(i => i.Item)
                .Where(i => i.Invoice.TypeId == Types.InvoiceTypes.Consumption.Id).Select(i => new ReportItem()
                {
                    Id = i.ItemId,
                    Name = i.Item.Name,
                    Amount = i.Amount,
                }).ToList();

            return _siriusContext.Items.Include(i => i.Dimension).ToList().Select(item =>
            {
                var incomingItems = incoming.Where(i => i.Id == item.Id && i.Amount > 0).Select(i => i.Amount);
                var consumptionItems = consumption.Where(i => i.Id == item.Id && i.Amount > 0).Select(i => i.Amount);

                var totalIncoming = incomingItems.Count() > 0 ? Math.Round(incomingItems.Aggregate((f, s) => f + s), 2) : 0;
                var totalConsumption = consumptionItems.Count() > 0 ? Math.Round(consumptionItems.Aggregate((f, s) => f + s), 2) : 0;

                return new ReportItemDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Incoming = totalIncoming,
                    Consumption = totalConsumption,
                    Total = Math.Round(totalIncoming - totalConsumption, 2),
                    Dimension = item.Dimension?.Name
                };

            }).Where(i => i.Incoming > 0 || i.Consumption > 0);
        }
    }
}

