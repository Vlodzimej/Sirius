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
        }

        public void DatabaseFill(List<Dimension> dimensions, List<Category> categories, List<Vendor> vendors, List<Item> items)
        {
            foreach (var dimension in dimensions)
            {
                _siriusContext.Dimensions.Add(dimension);
            }

            foreach (var category in categories)
            {
                _siriusContext.Categories.Add(category);
            }

            foreach (var vendor in vendors)
            {
                _siriusContext.Vendors.Add(vendor);
            }

            foreach (var item in items)
            {
                _siriusContext.Items.Add(item);
            }

        }
    }
}