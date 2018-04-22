using Sirius.Models;
using Sirius.DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sirius.Models
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(SiriusContext _siriusContext) : base(_siriusContext)
        {  }

        public IEnumerable<ItemDto> GetAll(
            Expression<Func<Item, bool>> filter = null,
            Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = null,
            string includeProperties = "")
        {
            var items = Get(filter, orderBy, includeProperties);
            var result = items.Select(i => new ItemDto()
            {
                Id = i.Id,
                Name = i.Name
            });
            return result;
        }

        public ItemDetailDto GetByID(Guid itemId)
        {
            var item = _siriusContext.Items
                .Include(i => i.Dimension)
                .Include(i => i.Category)
                .Select(i => new ItemDetailDto()
            {
                Id = i.Id,
                Name = i.Name,
                Dimension = i.Dimension,
                Category = i.Category,
                isCountless = i.isCountless
            }).SingleOrDefault(i => i.Id == itemId);
            return item;
        }
    }
}