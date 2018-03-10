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
            return unitOfWork.RegisterRepository.GetByID(id);
        }

        /// <summary>
        /// Получить список всех записей регистра
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Register> GetAllRegisters()
        {
            return unitOfWork.RegisterRepository.Get();
        }

        /// <summary>
        /// Удалить записть регистра по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteRegisterById(Guid id)
        {
            var register = unitOfWork.RegisterRepository.GetByID(id);
            if (register != null)
            {
                unitOfWork.RegisterRepository.Delete(register);
                unitOfWork.Save();
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

            unitOfWork.RegisterRepository.Insert(register);
            unitOfWork.Save();

            return unitOfWork.RegisterRepository.GetByID(register.Id) ?? null;
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
                unitOfWork.RegisterRepository.Update(register);
                unitOfWork.Save();
                return unitOfWork.RegisterRepository.GetByID(registerId);
            }
            return null;
        }
    }
}
