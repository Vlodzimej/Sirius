using Sirius.Models;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sirius.DAL.Repository.Contract
{
    interface IInvoiceRepository
    {
        /// <summary>
        /// Получение накладных по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<InvoiceListDto> GetAll(
            Expression<Func<Invoice, bool>> filter = null,
            Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Получение информации о накладной по идентификатору
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        object GetById(Guid invoiceId);

        /// <summary>
        /// Получение наклданой по идентификатору
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        Invoice GetByID(Guid invoiceId);

        /// <summary>
        /// Получение информации о типе накладной по идентификатору типа накладной
        /// </summary>
        /// <param name="invoiceTypeId"></param>
        /// <returns></returns>
        InvoiceType GetTypeById(Guid invoiceTypeId);

        /// <summary>
        /// Получение информации о типе накладной по алиасу
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        InvoiceType GetTypeByAlias(string alias);

        /// <summary>
        /// Получение типов накладных
        /// </summary>
        /// <returns></returns>
        IEnumerable<InvoiceType> GetTypes();

        bool UpdateDate(Guid id, DateTime date);
    }
}