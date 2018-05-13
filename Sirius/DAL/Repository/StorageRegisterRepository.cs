using Sirius.DAL.Repository.Contract;
using Sirius.Extends.Filters;
using Sirius.Helpers;
using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sirius.DAL.Repository
{
    public class StorageRegisterRepository : GenericRepository<StorageRegister>, IStorageRegisterRepository
    {
        public StorageRegisterRepository(SiriusContext _siriusContext) : base(_siriusContext)
        { }

        /// <summary>
        /// Получение хранимого регистра по идентификатору наименования и цене
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        public StorageRegister GetByItemIdAndCost(Guid itemId, decimal cost)
        {
            return _siriusContext.StorageRegisters.Where(sr => (sr.ItemId == itemId) && (sr.Cost == cost))?.FirstOrDefault();
        }
        /*
        public IEnumerable<object> GetStaticBatches(BatchFilter filter)
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
                    Batches = _siriusContext.StorageRegisters.Where(sr => sr.ItemId == filter.ItemId).Select(sr => new Batch { Amount = sr.Amount, Cost = sr.Cost }).AsEnumerable()
                };
                batchGroups.Add(batchGroup);

            });
            return batchGroups;
        }*/


    }
}
