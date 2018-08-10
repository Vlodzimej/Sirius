using Sirius.Models;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sirius.DAL.Repository.Contract
{
    interface IItemRepository
    {
        /// <summary>
        /// Получение наименований  по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<object> GetAll(
           Expression<Func<Item, bool>> filter = null,
           Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = null,
           string includeProperties = "");

        /// <summary>
        /// Получение наименования по идентификатору
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        object GetById(Guid itemId);
    }
}