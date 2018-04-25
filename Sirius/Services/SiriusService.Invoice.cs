﻿using Sirius.Models;
using System;
using System.Collections.Generic;
using Sirius.Models.Dtos;
using Sirius.Helpers;
using System.Linq.Expressions;
using System.Linq;
using Sirius.Extends.Filters;

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
            var invoice = _unitOfWork.InvoiceRepository.GetByID(invoiceId);
            var registers = _unitOfWork.RegisterRepository.GetByInvoiceId(invoiceId);

            // Взаимодействие с регистром накопления
            registers.ToList().ForEach(register => {
                var storageRegister = _unitOfWork.StorageRegisterRepository.GetByItemIdAndCost(register.ItemId, register.Cost);

                if(storageRegister != null)
                {
                    storageRegister.Amount += register.Amount * invoice.Factor;
                } else
                {
                    var newStorageRegister = new StorageRegister()
                    {
                        Id = Guid.NewGuid(),
                        createDate = DateTime.Now,
                        Amount = register.Amount,
                        Cost = register.Cost,
                        ItemId = register.ItemId
                    };
                    _unitOfWork.StorageRegisterRepository.Insert(newStorageRegister);
                }

                if(storageRegister.Amount == 0)
                {
                    _unitOfWork.StorageRegisterRepository.Delete(storageRegister);
                }
            });
            
            // Изменение свойств существующей накладной
            invoice.IsTemporary = false;
            invoice.IsFixed = true;
            _unitOfWork.Save();

            //UpdateInvoice(invoiceId, invoice);

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

        public string ChangeVendor(Guid invoiceId, Guid vendorId)
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
                result = "Изменён поставщик у накладной " + invoice.Name + ".";
            }
            else
            {
                result = "Поставщик не изменён.";
            }
            return result;
        }

        public string ChangeName(Guid invoiceId, string name)
        {
            string result;

            // Изменение свойств существующей накладной
            var invoice = _unitOfWork.InvoiceRepository.GetByID(invoiceId);
            invoice.Name = name;
            UpdateInvoice(invoiceId, invoice);

            // Проверка сохраненных изменений
            invoice = _unitOfWork.InvoiceRepository.GetByID(invoiceId);
            if (invoice != null && invoice.Name == name)
            {
                result = "Новое имя накладной: " + invoice.Name + ".";
            }
            else
            {
                result = "Имя накладной не изменено.";
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

        public IEnumerable<InvoiceListDto> GetInvoices(InvoiceFilter filter)
        {
            Expression<Func<Invoice, bool>> f = x =>
               (filter.Id != Guid.Empty ? x.Id == filter.Id : true) &&
               (filter.Name != null ? x.Name == filter.Name : true) &&
               (filter.VendorId != Guid.Empty ? x.VendorId == filter.VendorId : true) &&
               (filter.UserId != Guid.Empty ? x.UserId == filter.UserId : true) &&
               (filter.TypeId != Guid.Empty ? x.TypeId == filter.TypeId : true) &&
               (filter.StartDate != DateTime.MinValue ? x.CreateDate >= filter.StartDate : true) &&
               (filter.FinishDate != DateTime.MinValue ? x.CreateDate >= filter.FinishDate : true);
            return _unitOfWork.InvoiceRepository.GetAll(f);
        }
    }
}
