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
using Sirius.DAL.Repository.Contract;

namespace Sirius.Models
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(SiriusContext _siriusContext) : base(_siriusContext)
        { }

        /// <summary>
        /// Получение накладных по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceListDto> GetAll(
            Expression<Func<Invoice, bool>> filter = null,
            Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy = null,
            string includeProperties = "")
        {
            var invoices = _siriusContext.Invoices
            .Include(i => i.User)
            .Include(i => i.Vendor)
            .Where(filter)
            .OrderBy(i => i.CreateDate);

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

        /// <summary>
        /// Получение информации о накладной по идентификатору
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public object GetById(Guid invoiceId)
        {
            var invoice = GetByID(invoiceId);

            var registers = invoice.Registers.Select(r => new
            {
                r.Id,
                r.Cost,
                r.Amount,
                r.ItemId,
                CreateDate = r.Invoice.CreateDate.ToString("hhmmss"),
                r.Item.Name,
                Dimension = r.Item.Dimension.Name
            });

            var result = new 
            {
                Id = invoice.Id,
                Name = invoice.Name,
                UserId = invoice.UserId,
                UserFullName = invoice.User.FirstName+" "+invoice.User.LastName,
                VendorId = invoice.Vendor.Id,
                VendorName = invoice.Vendor.Name,
                Registers = registers,
                CreateDate = DateConverter.ConvertToStandardString(invoice.CreateDate),
                IsTemporary = invoice.IsTemporary,
                IsFixed = invoice.IsFixed,
                TypeId = invoice.TypeId
                
            };
            return result;
        }

        /// <summary>
        /// Получение наклданой по идентификатору
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Получение информации о типе накладной по идентификатору типа накладной
        /// </summary>
        /// <param name="invoiceTypeId"></param>
        /// <returns></returns>
        public InvoiceType GetTypeById(Guid invoiceTypeId)
        {
            return _siriusContext.InvoiceTypes.Find(invoiceTypeId);
        }

        /// <summary>
        /// Получение информации о типе накладной по алиасу
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public InvoiceType GetTypeByAlias(string alias)
        {
            return _siriusContext.InvoiceTypes.Where(it => it.Alias == alias).FirstOrDefault();
        }

        /// <summary>
        /// Получение типов накладных
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InvoiceType> GetTypes()
        {
            return _siriusContext.InvoiceTypes;
        }
               
    }
}