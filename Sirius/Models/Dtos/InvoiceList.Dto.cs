using System;

namespace Sirius.Models.Dtos
{
    public class InvoiceListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string CreateDate { get; set; }
    }
}