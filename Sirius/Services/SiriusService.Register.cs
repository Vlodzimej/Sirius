using Sirius.Helpers;
using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получить запись регистра по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Register GetRegisterById(Guid id)
        {
            using (var unitOfWork = new DAL.UnitOfWork())
            {
                return unitOfWork.RegisterRepository.GetByID(id);
            }
        }
        /// <summary>
        /// Получить запись регистра по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Register> GetRegistersByInvoiceId(Guid invoiceId)
        {
            //using (var unitOfWork = new DAL.UnitOfWork())
            //{
            var registers = _unitOfWork.RegisterRepository.GetByInvoiceId(invoiceId);
            return registers;
            //}
        }

        /// <summary>
        /// Получить запись регистра по Id наименования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<Batch>> GetRegisterByItemId(Guid id)
        {
            var filter = new MetaFilter() { itemId = id };
            return _unitOfWork.RegisterRepository.GetByFilter(filter);
        }

        /// <summary>
        /// Получить регистры по алиасу типа накладной (arrival/ )
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
        public IEnumerable<object> GetBatches(MetaFilter filter)
        {
            return _unitOfWork.RegisterRepository.GetBatches(filter);
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
                int count = 0;
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
            var registers = _unitOfWork.RegisterRepository.GetByInvoiceId(sourceInvoiceId);
            var newRegisters = new List<Register>();

            registers.ToList().ForEach(reg =>
            {
                var newRegister = new Register();
                var filter = new MetaFilter() { itemId = reg.ItemId };

                newRegister.Id = Guid.NewGuid();
                newRegister.InvoiceId = destinationInvoiceId;
                newRegister.ItemId = reg.ItemId;
                newRegister.Amount = reg.Amount;

                var batch = _unitOfWork.RegisterRepository.GetByFilter(filter).GetAwaiter().GetResult();
                if (batch.Count() > 0)
                {
                    newRegister.Cost = batch.FirstOrDefault().Cost;
                }
                else
                {
                    newRegister.Cost = 0;
                }

                _unitOfWork.RegisterRepository.Insert(newRegister);
            });

            _unitOfWork.Save();

            var result = GetRegistersByInvoiceId(destinationInvoiceId)
                .Select(r => new Register() { Id = r.Id, InvoiceId = r.InvoiceId, ItemId = r.ItemId, Amount = r.Amount, Cost = r.Cost});
            return result;

        }
    }
}
