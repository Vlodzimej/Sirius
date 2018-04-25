using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.DAL.Repository.Contract
{
    public interface IStorageRegisterRepository
    {
        StorageRegister GetByItemIdAndCost(Guid itemId, decimal cost);
    }
}
