using Sirius.Extends.Filters;
using Sirius.Models;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Services
{
    partial interface ISiriusService
    {
        /// <summary>
        /// Получить данные накладной по идентификатору
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        object GetInvoiceDetailDtoById(Guid invoiceId);

        /// <summary>
        /// Получить накладную по идентификатору
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        Invoice GetInvoiceById(Guid invoiceId);

        /// <summary>
        /// Получить список всех накладных
        /// </summary>
        /// <returns></returns>
        IEnumerable<InvoiceListDto> GetAllInvoices();

        /// <summary>
        /// Удалить накладную по идентификатору
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        string DeleteInvoiceById(Guid invoiceId);

        /// <summary>
        /// Добавить новую накладную
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        object AddInvoice(Invoice invoice);

        /// <summary>
        /// Обновить данные накладной
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        object UpdateInvoice(Guid invoiceId, Invoice invoice);

        /// <summary>
        /// Проведение накладной
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        string FixInvoice(Guid invoiceId);

        /// <summary>
        /// Изменить поставщика
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        string ChangeVendor(Guid invoiceId, Guid vendorId);

        /// <summary>
        /// Изменить название накладной
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        string ChangeName(Guid invoiceId, string name);

        /// <summary>
        /// Получение типа накладной по её идентификатору
        /// </summary>
        /// <param name="invoiceTypeId"></param>
        /// <returns></returns>
        InvoiceType GetInvoiceTypeByTypeId(Guid invoiceTypeId);

        /// <summary>
        /// Получение типа накладной по её алиасу
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        InvoiceType GetInvoiceTypeByAlias(string alias);

        /// <summary>
        /// Получение списка типов накладных
        /// </summary>
        /// <returns></returns>
        IEnumerable<InvoiceType> GetInvoiceTypes();

        /// <summary>
        ///  Получить накладные по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<InvoiceListDto> GetInvoices(InvoiceFilter filter);

        bool UpdateDate(Guid invoiceId, string date);
    }
}
