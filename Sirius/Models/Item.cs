using System;

namespace Sirius.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DimensionId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
    }
}
