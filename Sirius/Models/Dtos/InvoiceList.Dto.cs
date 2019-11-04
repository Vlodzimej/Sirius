using System;

namespace Sirius.Models.Dtos
{
    public class InvoiceListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserFullName { get; set; }
        public string Date { get; set; }
        public bool IsFixed { get; set; }
        public bool IsTemporary { get; set; }
    }
}
