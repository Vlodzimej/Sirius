using System;
using System.Collections.Generic;
using Sirius.Models;

namespace Sirius.Models.Dtos
{
    public class InvoiceDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public IEnumerable<Register> Registers { get; set; }
        public string Date { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsFixed { get; set; }
        public Guid TypeId { get; set; }
        public string Comment { get; set; }
    }
}
