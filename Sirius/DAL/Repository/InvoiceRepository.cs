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
using Sirius.Helpers;

namespace Sirius.Models
{
    public class InvoiceRepository : GenericRepository<Invoice>, IItemRepository
    {
        public InvoiceRepository(SiriusContext _siriusContext) : base(_siriusContext)
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
        public IEnumerable<InvoiceListDto> GetAll(
            Expression<Func<Invoice, bool>> filter = null,
            Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy = null,
            string includeProperties = "")
        {
            var invoices = _siriusContext.Invoices
            .Include(i => i.User)
            .Include(i => i.Vendor);

            if (invoices != null)
            {
                var result = invoices.Select(i => new InvoiceListDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    UserFullName = i.User.FirstName + " " + i.User.LastName,
                    CreateDate = DateConverter.ConvertToStandardString(i.CreateDate),
                    IsFixed = i.IsFixed,
                    IsTemporary = i.IsTemporary
                });
                return result;
            }
            return null;
        }

        public InvoiceDetailDto GetDetailDtoByID(Guid invoiceId)
        {
            var invoice = GetByID(invoiceId);

            var result = new InvoiceDetailDto()
            {
                Id = invoice.Id,
                Name = invoice.Name,
                UserId = invoice.UserId,
                UserFullName = invoice.User.FirstName+" "+invoice.User.LastName,
                VendorId = invoice.Vendor.Id,
                VendorName = invoice.Vendor.Name,
                Registers = invoice.Registers,
                CreateDate = DateConverter.ConvertToStandardString(invoice.CreateDate),
                IsTemporary = invoice.IsTemporary,
                IsFixed = invoice.IsFixed
            };
            return result;
        }

        public Invoice GetByID(Guid invoiceId)
        {
            var invoice = _siriusContext.Invoices
                .Include(i => i.User)
                .Include(i => i.Vendor)
                .Include(i => i.Registers)
                    .ThenInclude(r => r.Item)
                    .ThenInclude(i => i.Dimension)
                .Include(i => i.Registers)
                    .ThenInclude(r => r.Item)
                    .ThenInclude(i => i.Category)
                .SingleOrDefault(i => i.Id == invoiceId);

            return invoice;
        }
    }
}