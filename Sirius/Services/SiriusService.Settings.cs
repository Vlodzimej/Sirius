using System;
using System.Collections.Generic;
using System.Linq;
using Sirius.Models;
using Sirius.Helpers;
using System.Linq.Expressions;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получение значение настройки по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetSettingValueById(Guid id)
        {
            return _unitOfWork.SettingRepository.GetByID(id).Alias;
        }

        /// <summary>
        /// Получение значение настройки по идентификатору типа и алиасу
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public string GetSettingValueByTypeIdAndAlias(Guid typeId, string alias)
        {
            Expression<Func<Setting, bool>> f = x => (x.Alias == alias) && (x.TypeId == typeId);
            return _unitOfWork.SettingRepository.Get(f).FirstOrDefault()?.Value;
        }

        /// <summary>
        /// Откат базы данных
        /// </summary>
        public void RollbackDatabase()
        {
            var invoiceTypes = new List<InvoiceType>();
            var invoiceType = new InvoiceType()
            {
                Id = Types.InvoiceTypes.Arrival.Id,
                Factor = 1,
                Name = "Приход",
                Alias = "arrival"
            };
            invoiceTypes.Add(invoiceType);

            invoiceType = new InvoiceType()
            {
                Id = Types.InvoiceTypes.Consumption.Id,
                Factor = (-1),
                Name = "Расход",
                Alias = "consumption"
            };
            invoiceTypes.Add(invoiceType);

            invoiceType = new InvoiceType()
            {
                Id = Types.InvoiceTypes.Writeoff.Id,
                Factor = (-1),
                Name = "Списание",
                Alias = "writeoff"
            };
            invoiceTypes.Add(invoiceType);

            invoiceType = new InvoiceType()
            {
                Id = Types.InvoiceTypes.Template.Id,
                Factor = 0,
                Name = "Услуга",
                Alias = "template"
            };
            invoiceTypes.Add(invoiceType);

            // Создание списка категорий
            var categoryPrototypes = new List<Category>();
            var category = new Category() {
                Id = Guid.Parse("B5E7EACE-1AED-4173-B383-32C7F9643F44"),
                Name = "Расходные материалы"
            };
            categoryPrototypes.Add(category);

            // Создание списка единиц измерения
            var dimensionPrototypes = new List<Dimension>();

            var dimension = new Dimension()
            {
                Id = Guid.Parse("3BF5C95A-2002-457F-8299-86BF25839480"),
                Name = "шт."
            };
            dimensionPrototypes.Add(dimension);
            dimension = new Dimension()
            {
                Id = Guid.Parse("E35BB3F7-E944-4E87-BC59-F4B5A1F5C219"),
                Name = "пар."
            };
            dimensionPrototypes.Add(dimension);
            dimension = new Dimension()
            {
                Id = Guid.Parse("6358D37D-FC67-48FE-85EA-DB404106BD99"),
                Name = "л."
            };
            dimensionPrototypes.Add(dimension);
            dimension = new Dimension()
            {
                Id = Guid.Parse("41B62CF2-B534-4619-BD22-68B3756FFC04"),
                Name = "мл."
            };
            dimensionPrototypes.Add(dimension);

            // Создание списка поставщиков
            var vendorPrototypes = new List<Vendor>();
            var vendor = new Vendor()
            {
                Id = DefaultValues.Vendor.Primary.Id,
                Name = "Основной поставщик",
                Address = "г. Калуга",
                Contact = "тел.: 8(4842)77-77-77"
            };
            vendorPrototypes.Add(vendor);

            // Создание списка наименований
            var itemPrototypes = new List<Item>();
            var item = new Item()
            {
                Id = Guid.Parse("C4C00A31-8661-4C1B-A699-651AF03097FE"),
                Name = "Перчатки резиновые",
                DimensionId = dimensionPrototypes.Where(d => d.Id == Guid.Parse("E35BB3F7-E944-4E87-BC59-F4B5A1F5C219")).FirstOrDefault().Id,
                CategoryId = categoryPrototypes.FirstOrDefault().Id
            };
            itemPrototypes.Add(item);

            // Создание списка настроек
            var settingsPrototypes = new List<Setting>();
            var setting = new Setting()
            {
                Id = Guid.NewGuid(),
                Name = "Префикс прихода",
                Value = "Пн",
                TypeId = Types.SettingsTypes.Invoice.Prefix.Id,
                Alias = "arrival"
            };
            settingsPrototypes.Add(setting);

            setting = new Setting()
            {
                Id = Guid.NewGuid(),
                Name = "Префикс расхода",
                Value = "Рн",
                TypeId = Types.SettingsTypes.Invoice.Prefix.Id,
                Alias = "consumption"
            };
            settingsPrototypes.Add(setting);

            setting = new Setting()
            {
                Id = Guid.NewGuid(),
                Name = "Префикс списания",
                Value = "Сп",
                TypeId = Types.SettingsTypes.Invoice.Prefix.Id,
                Alias = "writeoff"
            };
            settingsPrototypes.Add(setting);

            setting = new Setting()
            {
                Id = Guid.NewGuid(),
                Name = "Префикс шаблона",
                Value = "Шбл",
                TypeId = Types.SettingsTypes.Invoice.Prefix.Id,
                Alias = "template"
            };
            settingsPrototypes.Add(setting);

            _unitOfWork.SettingRepository.DatabaseDropTables();
            _unitOfWork.Save();

            _unitOfWork.SettingRepository.DatabaseFill(invoiceTypes, dimensionPrototypes, categoryPrototypes, vendorPrototypes, itemPrototypes, settingsPrototypes);
            _unitOfWork.Save();
        }

    }
}
