using Sirius.Models;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sirius.DAL.Repository.Contract
{
    interface ILogRepository
    {
        /// <summary>
        /// Получение логов по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<object> GetAll(
           Expression<Func<Item, bool>> filter = null,
           Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = null,
           string includeProperties = "");


        void Create(string content, Guid userId);


    }
}