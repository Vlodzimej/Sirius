﻿using Sirius.Models;
using System;
using System.Collections.Generic;
using Sirius.Models.Dtos;
using Sirius.Helpers;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получить накладную dto по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InvoiceDetailDto GetInvoiceDetailDtoById(Guid id)
        {
            return _unitOfWork.InvoiceRepository.GetDetailDtoByID(id);
        }

        /// <summary>
        /// Получить накладную по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Invoice GetInvoiceById(Guid id)
        {
            return _unitOfWork.InvoiceRepository.GetByID(id);
        }


        /// <summary>
        /// Получить список всех накладных
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InvoiceListDto> GetAllInvoices()
        {
            return _unitOfWork.InvoiceRepository.GetAll();
        }

        /// <summary>
        /// Удалить накладную по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteInvoiceById(Guid id)
        {
            var invoice = _unitOfWork.InvoiceRepository.GetByID(id);
            if (invoice != null)
            {
                _unitOfWork.InvoiceRepository.Delete(invoice);
                _unitOfWork.Save();
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
            var newInvoiceId = Guid.NewGuid();
            var newInvoice = new Invoice()
            {
                Id = newInvoiceId,
                Name = "",
                UserId = invoice.UserId,
                VendorId = DefaultValues.Vendor.Primary.Id,
                CreateDate = DateTime.Now,
                IsTemporary = true,
                IsFixed = false
            };

            _unitOfWork.InvoiceRepository.Insert(newInvoice);
            _unitOfWork.Save();

            var addedInvoice = _unitOfWork.InvoiceRepository.GetByID(newInvoiceId) ?? null;

            if (addedInvoice != null)
            {
                // Здесь должен быть запрос префикса для накладной из таблицы Settings
                var prefix = "Пн";
                var year = DateTime.Now.ToString("yy");
                var number = addedInvoice.CreateDate.ToString("hhmmss");

                addedInvoice.Name = $"{prefix}-{year}/{number}";

                _unitOfWork.InvoiceRepository.Update(addedInvoice);
                _unitOfWork.Save();

                return _unitOfWork.InvoiceRepository.GetDetailDtoByID(newInvoiceId) ?? null;
            }
            return null;
        }

        /// <summary>
        /// Обновить данные накладной
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public InvoiceDetailDto UpdateInvoice(Guid invoiceId, Invoice invoice)
        {
            if (invoiceId != null && invoice != null && invoiceId == invoice.Id)
            {
                _unitOfWork.InvoiceRepository.Update(invoice);
                _unitOfWork.Save();
                return _unitOfWork.InvoiceRepository.GetDetailDtoByID(invoiceId);

            }
            return null;
        }

        /// <summary>
        /// Проведение накладной
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public string FixInvoice(Guid invoiceId)
        {
            string result;

            // Изменение свойств существующей накладной
            var invoice = _unitOfWork.InvoiceRepository.GetByID(invoiceId);
            invoice.IsTemporary = false;
            invoice.IsFixed = true;
            UpdateInvoice(invoiceId, invoice);

            // Проверка сохраненных изменений
            invoice = _unitOfWork.InvoiceRepository.GetByID(invoiceId);
            if (invoice != null && invoice.IsFixed == true)
            {
                result = "Накладная " + invoice.Name + " успешно проведена!";
            }
            else
            {
                result = "Накладная не проведена.";
            }
            return result;
        }
    }
}
