using Sirius.Models;
using Sirius.DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sirius.DAL.Repository.Contract;

namespace Sirius.Models
{
    public class SettingRepository : GenericRepository<Setting>, ISettingRepository
    {
        public SettingRepository(SiriusContext context) : base(context)
        { }

        /// <summary>
        /// Удаление данных таблиц
        /// </summary>
        public void DatabaseDropTables()
        {
            _siriusContext.InvoiceTypes.RemoveRange(_siriusContext.InvoiceTypes);
            _siriusContext.Categories.RemoveRange(_siriusContext.Categories);
            _siriusContext.Dimensions.RemoveRange(_siriusContext.Dimensions);
            _siriusContext.Invoices.RemoveRange(_siriusContext.Invoices);
            _siriusContext.Items.RemoveRange(_siriusContext.Items);
            _siriusContext.Vendors.RemoveRange(_siriusContext.Vendors);
            _siriusContext.Registers.RemoveRange(_siriusContext.Registers);
            _siriusContext.StorageRegisters.RemoveRange(_siriusContext.StorageRegisters);
            _siriusContext.Settings.RemoveRange(_siriusContext.Settings);
        }

        /// <summary>
        /// Добавление первичных данных в базу данных
        /// </summary>
        /// <param name="invoiceTypes"></param>
        /// <param name="dimensions"></param>
        /// <param name="categories"></param>
        /// <param name="vendors"></param>
        /// <param name="items"></param>
        /// <param name="settings"></param>
        public void DatabaseFill(
            List<InvoiceType> invoiceTypes,
            List<Dimension> dimensions, 
            List<Category> categories, 
            List<Vendor> vendors, 
            List<Item> items,
            List<Setting> settings)
        {
            // Добавление типов накладных
            foreach (var invoiceType in invoiceTypes)
            {
                _siriusContext.InvoiceTypes.Add(invoiceType);
            }
            // Добавление единиц измерений
            foreach (var dimension in dimensions)
            {
                _siriusContext.Dimensions.Add(dimension);
            }

            // Добавление категорий
            foreach (var category in categories)
            {
                _siriusContext.Categories.Add(category);
            }

            // Добавление поставщиков
            foreach (var vendor in vendors)
            {
                _siriusContext.Vendors.Add(vendor);
            }

            // Добавление наименований
            foreach (var item in items)
            {
                _siriusContext.Items.Add(item);
            }

            // Добавление настроек
            foreach (var setting in settings)
            {
                _siriusContext.Settings.Add(setting);
            }
        }
    }
}