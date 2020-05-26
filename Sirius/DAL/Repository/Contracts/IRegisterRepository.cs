using Sirius.Extends.Filters;
using Sirius.Helpers;
using Sirius.Models;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sirius.DAL.Repository.Contract
{
    interface IRegisterRepository
    {
        /// <summary>
        /// Получение регистров по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<object> GetAll(
       Expression<Func<Register, bool>> filter = null,
       Func<IQueryable<Register>, IOrderedQueryable<Register>> orderBy = null,
       string includeProperties = "");

        /// <summary>
        /// Получение регистров по идентификатору накладной
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        IEnumerable<Register> GetByInvoiceId(Guid invoiceId);

        /// <summary>
        /// Получение регистров по алиасу типа накладной
        /// </summary>
        /// <param name="typeAlias"></param>
        /// <returns></returns>
        IEnumerable<object> GetByTypeAlias(string typeAlias);

        /// <summary>
        /// Получение динамических остатков по критериям фильтрации
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<Batch>> GetDynamicBatchesByFilter(BatchFilter filter);


        /// <summary>
        /// Получение статических остатков по критериям фильтрации из регистра накопления (StorageRegister)
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<Batch> GetStaticBatchesByFilter(BatchFilter filter);

        /// <summary>
        /// Получение сгруппированного по наименованиям списка остатков 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<BatchGroup> GetBatches(BatchFilter filter);

        IEnumerable<ReportItemDto> GetReportItems();

    }
}