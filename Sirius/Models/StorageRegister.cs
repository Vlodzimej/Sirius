using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Models
{
    public class StorageRegister
    {
        public Guid Id { get; set; }
        public DateTime createDate { get; set; }
        public double Amount { get; set; }
        public decimal Cost { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
    }
}
