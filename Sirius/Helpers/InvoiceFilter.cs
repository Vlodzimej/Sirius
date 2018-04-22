﻿using System;

namespace Sirius.Helpers
{
    public class InvoiceFilter
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public Guid VendorId { get; set; }
        public Guid UserId { get; set; }
        public Guid TypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
