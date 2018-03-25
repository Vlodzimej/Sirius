using Sirius.Models;
using System;
using System.Collections.Generic;

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
        /// Добавить новую запись в регистр
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        public Register AddRegister(Register register)
        {
            register.Id = Guid.NewGuid();

            _unitOfWork.RegisterRepository.Insert(register);
            _unitOfWork.Save();

            return _unitOfWork.RegisterRepository.GetByID(register.Id) ?? null;
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
