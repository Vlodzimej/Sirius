using Sirius.DAL.Repository.Contract;
using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.DAL.Repository
{
    public class StorageRegisterRepository : GenericRepository<StorageRegister>, IStorageRegisterRepository
    {
        public StorageRegisterRepository(SiriusContext _siriusContext) : base(_siriusContext)
        { }

        public StorageRegister GetByItemIdAndCost(Guid itemId, decimal cost)
        {
            return _siriusContext.StorageRegisters.Where(sr => (sr.ItemId == itemId) && (sr.Cost == cost))?.FirstOrDefault();
        }
    }
}
