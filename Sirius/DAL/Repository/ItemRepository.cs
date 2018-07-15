using Sirius.Models;
using Sirius.DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sirius.DAL.Repository.Contract;

namespace Sirius.Models
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(SiriusContext _siriusContext) : base(_siriusContext)
        { }

        /// <summary>
        /// Получение наименований  по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(
            Expression<Func<Item, bool>> filter = null,
            Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = null,
            string includeProperties = "")
        {
            var items = Get(filter, orderBy, includeProperties);
            var result = items.Select(i => new
            {
                Id = i.Id,
                Name = i.Name,
                MinimumLimit = i.MinimumLimit
            });
            return result;
        }

        /// <summary>
        /// Получение наименования по идентификатору
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public object GetByID(Guid itemId)
        {
            var item = _siriusContext.Items
                .Include(i => i.Dimension)
                .Include(i => i.Category)
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    i.Dimension,
                    i.Category,
                    i.IsCountless,
                    i.MinimumLimit
                }).SingleOrDefault(i => i.Id == itemId);
            return item;
        }
    }
}