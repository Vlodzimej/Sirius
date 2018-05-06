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

        public void DatabaseDropTables()
        {
            _siriusContext.Categories.RemoveRange(_siriusContext.Categories);
            _siriusContext.Dimensions.RemoveRange(_siriusContext.Dimensions);
            _siriusContext.Invoices.RemoveRange(_siriusContext.Invoices);
            _siriusContext.Items.RemoveRange(_siriusContext.Items);
            _siriusContext.Vendors.RemoveRange(_siriusContext.Vendors);
            _siriusContext.Registers.RemoveRange(_siriusContext.Registers);
            _siriusContext.StorageRegisters.RemoveRange(_siriusContext.StorageRegisters);
        }

        public void DatabaseFill(
            List<Dimension> dimensions, 
            List<Category> categories, 
            List<Vendor> vendors, 
            List<Item> items,
            List<Setting> settings)
        {
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