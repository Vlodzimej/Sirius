using Sirius.Models;
using Sirius.DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Globalization;

namespace Sirius.Models
{
    public class InvoiceRepository : GenericRepository<Invoice>, IItemRepository
    {
        public InvoiceRepository(SiriusContext context) : base(context)
        { }

        /// <summary>
        /// Получение списка накладных
        /// </summary>
        /// <param name="Expression<Func<Invoice"></param>
        /// <param name="filter"></param>
        /// <param name="Func<IQueryable<Invoice>"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns>Invoice List Items</returns>
        public IEnumerable<InvoiceDto> GetAll(
            Expression<Func<Invoice, bool>> filter = null,
            Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy = null,
            string includeProperties = "")
        {
            var invoices = context.Invoice
            .Include(i => i.User)
            .Include(i => i.Vendor);

            if (invoices != null)
            {

                var result = invoices.Select(i => new InvoiceDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Author = i.User.FirstName + " " + i.User.LastName,
                    CreateDate = i.CreateDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)
                });
                return result;
            }
            return null;
        }
    }
}