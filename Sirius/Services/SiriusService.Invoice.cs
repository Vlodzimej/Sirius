using Sirius.Models;
using System;
using System.Collections.Generic;
using Sirius.Models.Dtos;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получить накладную по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvoiceDetailDto GetInvoiceById(Guid id)
        {
            return unitOfWork.InvoiceRepository.GetByID(id);
        }

        /// <summary>
        /// Получить список всех накладных
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InvoiceListDto> GetAllInvoices()
        {
            return unitOfWork.InvoiceRepository.GetAll();
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
        public InvoiceDetailDto AddInvoice(Invoice invoice)
        {
            var newInvoice = new Invoice()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                UserId = Guid.Parse("c5efcdc4-cc97-481f-89ed-7614da4e6541"),
                VendorId = Guid.Parse("7c32414b-665d-4241-8bf1-0239ad7344fa"),
                CreateDate = DateTime.Now,
                IsTemporary = true,
                IsRecorded = false
            };

            unitOfWork.InvoiceRepository.Insert(newInvoice);
            unitOfWork.Save();

            return unitOfWork.InvoiceRepository.GetByID(invoice.Id) ?? null;
        }

        /// <summary>
        /// Обновить данные накладной
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public InvoiceDetailDto UpdateInvoice(Guid invoiceId, Invoice invoice)
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
