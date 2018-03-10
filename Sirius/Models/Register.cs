using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Models
{
    /// <summary>
    /// Регистры предметов
    /// </summary>
    public class Register
    {
        public Guid Id { get; set; }

        public decimal Cost { get; set; }

        public Guid ItemId { get; set; }

        public Item Item { get; set; }

        public Guid InvoiceId { get; set; }

        public Invoice Invoice { get; set; }
    }
}
