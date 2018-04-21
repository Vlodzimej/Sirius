using System;

namespace Sirius.Helpers
{
    public class Filter
    {
        public Guid categoryId { get; set; }
        public Guid itemId { get; set; }
        public Guid vendorId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime finishDate { get; set; }
    }
}
