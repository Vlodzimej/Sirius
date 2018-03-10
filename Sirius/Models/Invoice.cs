using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Models
{
    /// <summary>
    /// Накладные
    /// </summary>
    public class Invoice
    {
        public Guid Id { get; set; }

        public Guid VendorId { get; set; }

        public Vendor Vendor { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public List<Register> Register { get; set; }

        public bool IsRecorded { get; set; } 

        public DateTime CreateDate { get; set; }
    }
}
