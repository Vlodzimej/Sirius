using Sirius.Models;
using System;
using System.Collections.Generic;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получить накладную по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Invoice GetInvoiceById(Guid id)
        {
            return unitOfWork.InvoiceRepository.GetByID(id);
        }

        /// <summary>
        /// Получить список всех накладных
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Invoice> GetAllInvoices()
        {
            return unitOfWork.InvoiceRepository.Get();
        }

        /// <summary>
        /// Удалить накладную по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteInvoiceById(Guid id)
        {
            var invoice = unitOfWork.InvoiceRepository.GetByID(id);
            if (invoice != null)
            {
                unitOfWork.InvoiceRepository.Delete(invoice);
                unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавить новую накладную
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public Invoice AddRegister(Invoice invoice)
        {
            invoice.Id = Guid.NewGuid();

            unitOfWork.InvoiceRepository.Insert(invoice);
            unitOfWork.Save();

            return unitOfWork.InvoiceRepository.GetByID(invoice.Id) ?? null;
        }

        /// <summary>
        /// Обновить запись регистра
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public Invoice UpdateRegister(Guid invoiceId, Invoice invoice)
        {
            if (invoiceId == invoice.Id)
            {
                unitOfWork.InvoiceRepository.Update(invoice);
                unitOfWork.Save();
                return unitOfWork.InvoiceRepository.GetByID(invoiceId);
            }
            return null;
        }
    }
}
