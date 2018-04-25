using Microsoft.EntityFrameworkCore;
using Sirius.DAL;
using Sirius.Helpers;
using Sirius.Extends.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirius.DAL.Repository.Contract;

namespace Sirius.Models
{
    public class RegisterRepository : GenericRepository<Register>, IRegisterRepository
    {
        public RegisterRepository(SiriusContext _siriusContext) : base(_siriusContext)
        { }

        public IEnumerable<Register> GetByInvoiceId(Guid invoiceId)
        {
            return _siriusContext.Registers.Where(r => r.InvoiceId == invoiceId);
        }

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
                        CreateDate = DateConverter.ConvertToStandardString(r.Invoice.CreateDate),
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
        public async Task<IEnumerable<Batch>> GetBatchesByFilter(BatchFilter filter)
        {
            List<Batch> result = new List<Batch>();

            var arrivalRegisters1 = await _siriusContext.Registers
                .Include(r => r.Item)
                .Include(r => r.Invoice)
                .ToListAsync();

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
                .OrderBy(r => r.Invoice.CreateDate)
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
                    Amount = amount,
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
                    (r.Item.isCountless == false) &&
                    (filter.CategoryId != Guid.Empty ? r.Item.CategoryId == filter.CategoryId : true) &&
                    (filter.VendorId != Guid.Empty ? r.Invoice.VendorId == filter.VendorId : true))
                .OrderBy(r => r.Invoice.CreateDate)
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
                    Amount = amount,
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

        /// <summary>
        /// Получение сгруппированного по наименованиям списка остатков 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<object> GetBatches(BatchFilter filter)
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
                BatchGroup batchGroup = new BatchGroup()
                {
                    Name = i.Name,
                    Batches = GetBatchesByFilter(filter).Result.ToList()
                };
                batchGroups.Add(batchGroup);

            });

            return batchGroups;
        }
    }
}