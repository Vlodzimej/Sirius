using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Добавить массив регистров
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public dynamic AddRegisters(Register[] registers)
        {
            int count = 0;
            if (registers != null)
            {
                foreach (var register in registers)
                {
                    register.Id = Guid.NewGuid();
                    register.Item = null;
                    _unitOfWork.RegisterRepository.Insert(register);
                    count++;
                }

                _unitOfWork.Save();

                return count;
            }
            return null;
        }

        /// <summary>
        /// Обновить запись регистра
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
    }
}
