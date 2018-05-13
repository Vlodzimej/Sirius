using Sirius.Extends.Filters;
using Sirius.Helpers;
using Sirius.Models;
using System;
using System.Collections.Generic;

namespace Sirius.DAL.Repository.Contract
{
    public interface IStorageRegisterRepository
    {
        /// <summary>
        /// Получение хранимого регистра по идентификатору наименования и цене
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        StorageRegister GetByItemIdAndCost(Guid itemId, decimal cost);
    }
}
