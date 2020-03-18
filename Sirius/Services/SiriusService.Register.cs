using Sirius.Helpers;
using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirius.Extends.Filters;
using System.Linq.Expressions;
using System.IO;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получение регистров по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public object GetRegistersByFilter(MetaFilter filter)
        {
            if (filter.FinishDate != DateTime.MinValue)
            {
                filter.FinishDate = filter.FinishDate.AddMinutes(59).AddHours(23);
            }
            Expression<Func<Register, bool>> f = x =>
            (filter.ItemId != Guid.Empty ? x.ItemId == filter.ItemId : true) &&
            (filter.CategoryId != Guid.Empty ? x.Item.CategoryId == filter.CategoryId : true) &&
            (filter.StartDate != DateTime.MinValue ? x.Invoice.Date >= filter.StartDate : true) &&
            (filter.FinishDate != DateTime.MinValue ? x.Invoice.Date <= filter.FinishDate : true) &&
            (filter.TypeId != Guid.Empty ? x.Invoice.TypeId == filter.TypeId : true) &&
            (x.Invoice.IsFixed == true);

            return _unitOfWork.RegisterRepository.GetAll(f);
        }
        /// <summary>
        /// Получить запись регистра по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Register GetRegisterById(Guid id)
        {
            return _unitOfWork.RegisterRepository.GetByID(id);

        }
        /// <summary>
        /// Получить запись регистра по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Register> GetRegistersByInvoiceId(Guid invoiceId)
        {
            var registers = _unitOfWork.RegisterRepository.GetByInvoiceId(invoiceId);
            return registers;
        }

        /// <summary>
        /// Получить запись регистра по Id наименования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<Batch>> GetBatchesByItemId(Guid id)
        {
            var filter = new BatchFilter() { ItemId = id };
            return _unitOfWork.RegisterRepository.GetDynamicBatchesByFilter(filter);
        }

        /// <summary>
        /// Получить регистры по алиасу типа накладной (arrival/consumption/writeoff)
        /// </summary>
        /// <param name="typeAlias"></param>
        /// <returns></returns>
        public IEnumerable<object> GetRegisterByTypeAlias(string typeAlias)
        {
            return _unitOfWork.RegisterRepository.GetByTypeAlias(typeAlias);
        }

        /// <summary>
        /// Получить остатки согласно критериям фильтрации
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<object> GetBatches(BatchFilter filter)
        {
            var batches = _unitOfWork.RegisterRepository.GetBatches(filter);

            var fileDownloadName = "report.xlsx";
            using (var package = CreateExcelPackage(batches))
            {
                package.SaveAs(new FileInfo(fileDownloadName));
            }
            return batches;
        }

        /// <summary>
        /// Получить 1 остаток согласно критериям фильтрации
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public object GetBatch(BatchFilter filter)
        {
            return _unitOfWork.RegisterRepository.GetDynamicBatchesByFilter(filter).Result.FirstOrDefault();
        }

        /// <summary>
        /// Получить список всех записей регистра
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Register> GetAllRegisters()
        {
            return _unitOfWork.RegisterRepository.Get();
        }

        /// <summary>
        /// Удалить записть регистра по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteRegisterById(Guid id)
        {
            var register = _unitOfWork.RegisterRepository.GetByID(id);
            if (register != null)
            {
                _unitOfWork.RegisterRepository.Delete(register);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Удалить массив регистров по их Ids
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteRegistersById(Guid[] ids)
        {
            if (ids != null && ids.Count() > 0)
            {
                foreach (var id in ids)
                {
                    var register = _unitOfWork.RegisterRepository.GetByID(id);
                    if (register != null)
                    {
                        _unitOfWork.RegisterRepository.Delete(register);
                    }
                }
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавить новый регистр
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public Register AddRegister(Register register)
        {
            if (register != null)
            {
                register.Id = Guid.NewGuid();
                register.Item = null;

                _unitOfWork.RegisterRepository.Insert(register);

                _unitOfWork.Save();

                return _unitOfWork.RegisterRepository.GetByID(register.Id) ?? null;
            }
            return null;
        }

        /// <summary>
        /// Добавление массива регистров
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public IEnumerable<Register> AddRegisters(Register[] registers)
        {
            int count = 0;
            if (registers != null)
            {
                foreach (var register in registers)
                {
                    register.Id = Guid.NewGuid();
                    register.Invoice = null;
                    register.Item = null;
                    _unitOfWork.RegisterRepository.Insert(register);
                    count++;
                }

                _unitOfWork.Save();

                return registers;
            }
            return null;
        }

        /// <summary>
        /// Обновление записи регистра
        /// </summary>
        /// <param name="registerId"></param>
        /// <param name="register"></param>
        /// <returns></returns>
        public Register UpdateRegister(Guid registerId, Register register)
        {
            if (registerId == register.Id)
            {
                _unitOfWork.RegisterRepository.Update(register);
                _unitOfWork.Save();
                return _unitOfWork.RegisterRepository.GetByID(registerId);
            }
            return null;
        }

        /// <summary>
        /// Копирование регистров в целевую накладную
        /// </summary>
        /// <param name="sourceInvoiceId"></param>
        /// <param name="destinationInvoiceId"></param>
        /// <returns></returns>
        public IEnumerable<Register> CopyRegisters(Guid sourceInvoiceId, Guid destinationInvoiceId)
        {
            //ПОЯСНЕНИЕ:
            //Item - позиция
            //Batch - остаток
            //Invoice - накладная
            //Register - регистр
            //Получаем регисты их исходной накладной (шаблона)
            var registers = _unitOfWork.RegisterRepository.GetByInvoiceId(sourceInvoiceId);

            registers.ToList().ForEach(reg =>
            {
                // Получаем остатки по позиции и остатку текущего регистра
                var filter = new BatchFilter() { ItemId = reg.ItemId };
                var batches = _unitOfWork.RegisterRepository.GetDynamicBatchesByFilter(filter).GetAwaiter().GetResult();
                // Вызываем метод копирования
                AddCopiedRegister(new Register() { InvoiceId = destinationInvoiceId, ItemId = reg.ItemId, Amount = reg.Amount }, batches);
            });

            _unitOfWork.Save();

            var result = GetRegistersByInvoiceId(destinationInvoiceId)
                .Select(r => new Register() { Id = r.Id, InvoiceId = r.InvoiceId, ItemId = r.ItemId, Amount = r.Amount, Cost = r.Cost });
            return result;

        }

        /// <summary>
        /// Добавление копированных записей регистра
        /// </summary>
        /// <param name="register"></param>
        /// <param name="batches"></param>
        private void AddCopiedRegister(Register register, IEnumerable<Batch> batches)
        {
            // Проверяем наличие остатков по текущей позиции регистра
            if (batches.Count() > 0)
            {
                foreach (var batch in batches)
                {
                    if (batch.Amount >= register.Amount)
                    {
                        // Расход не превышает остаток, поэтому просто записываем новый регистр
                        var newRegister = new Register()
                        {
                            Id = Guid.NewGuid(),
                            InvoiceId = register.InvoiceId,
                            ItemId = register.ItemId,
                            Amount = register.Amount,
                            Cost = batch.Cost
                        };
                        register.Amount = 0;
                        _unitOfWork.RegisterRepository.Insert(newRegister);
                        break;
                    }
                    else
                    {
                        // Расход больше остатка, поэтому регистр добавляется с количеством текущего остатка
                        var newRegister = new Register()
                        {
                            Id = Guid.NewGuid(),
                            InvoiceId = register.InvoiceId,
                            ItemId = register.ItemId,
                            Amount = batch.Amount,
                            Cost = batch.Cost
                        };
                        // Из кол-ва в регисте вычитается кол-во остатока
                        register.Amount -= batch.Amount;
                        _unitOfWork.RegisterRepository.Insert(newRegister);
                    }
                }
                if (register.Amount > 0)
                {
                    // Итоговый расход больше всех остатков по позиции, поэтому регистр добавляется с отрицательным значением цены
                    var newRegister = new Register()
                    {
                        Id = Guid.NewGuid(),
                        InvoiceId = register.InvoiceId,
                        ItemId = register.ItemId,
                        Amount = register.Amount,
                        Cost = (-1)
                    };
                    _unitOfWork.RegisterRepository.Insert(newRegister);
                }
            }
            else
            {
                var item = _unitOfWork.ItemRepository.GetById(register.ItemId);
                var newRegister = new Register()
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = register.InvoiceId,
                    ItemId = register.ItemId,
                    Amount = register.Amount,
                    Cost = (-1)
                };
                _unitOfWork.RegisterRepository.Insert(newRegister);
            }


        }
    }
}
