using Sirius.Models;
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
        public object GetInvoiceDetailDtoById(Guid invoiceId)
        {
            return _unitOfWork.InvoiceRepository.GetById(invoiceId);
        }

        /// <summary>
        /// Получить накладную по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Invoice GetInvoiceById(Guid invoiceId)
        {
            return _unitOfWork.InvoiceRepository.GetByID(invoiceId);
        }

        /// <summary>
        /// Получить список  накладных определённого типа
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InvoiceListDto> GetByTypeId(Guid typeId)
        {
            return _unitOfWork.InvoiceRepository.GetAll(x => x.TypeId == typeId);
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
        public string DeleteInvoiceById(Guid invoiceId)
        {
            string result = "";
            var invoice = _unitOfWork.InvoiceRepository.GetByID(invoiceId);
            if (invoice != null && !invoice.IsFixed)
            {
                _unitOfWork.InvoiceRepository.Delete(invoice);
                _unitOfWork.Save();
                result = "Накладная удалена.";
            }
            if (invoice.IsFixed)
            {
                result = "Накладная проведена. Удаление невозможно.";
            }
            return result;
        }

        /// <summary>
        /// Добавить новую накладную
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public object AddInvoice(Invoice invoice)
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
                IsFixed = false,
                TypeId = invoice.TypeId,
                Comment = invoice.Comment,
                Factor = invoice.Factor
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

                return _unitOfWork.InvoiceRepository.GetById(newInvoiceId) ?? null;
            }
            return null;
        }

        /// <summary>
        /// Обновить данные накладной
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public object UpdateInvoice(Guid invoiceId, Invoice invoice)
        {
            if (invoiceId != null && invoice != null && invoiceId == invoice.Id)
            {
                _unitOfWork.InvoiceRepository.Update(invoice);
                _unitOfWork.Save();
                return _unitOfWork.InvoiceRepository.GetById(invoiceId);

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

        public string SetVendor(Guid invoiceId, Guid vendorId)
        {
            string result;

            // Изменение свойств существующей накладной
            var invoice = _unitOfWork.InvoiceRepository.GetByID(invoiceId);
            invoice.VendorId = vendorId;
            UpdateInvoice(invoiceId, invoice);

            // Проверка сохраненных изменений
            invoice = _unitOfWork.InvoiceRepository.GetByID(invoiceId);
            if (invoice != null && invoice.VendorId == vendorId)
            {
                result = "Накладная " + invoice.Name + " успешно обновлена!";
            }
            else
            {
                result = "Накладная не обновлена.";
            }
            return result;
        }

        /// <summary>
        /// Получение типа накладной по её идентификатору
        /// </summary>
        /// <param name="invoiceTypeId"></param>
        /// <returns></returns>
        public InvoiceType GetInvoiceTypeByTypeId(Guid invoiceTypeId)
        {
            return _unitOfWork.InvoiceRepository.GetTypeById(invoiceTypeId);
        }

        /// <summary>
        /// Получение типа накладной по её алиасу
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public InvoiceType GetInvoiceTypeByAlias(string alias)
        {
            return _unitOfWork.InvoiceRepository.GetTypeByAlias(alias);
        }

        /// <summary>
        /// Получение списка типов накладных
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InvoiceType> GetInvoiceTypes()
        {
            return _unitOfWork.InvoiceRepository.GetTypes();
        }
    }
}
