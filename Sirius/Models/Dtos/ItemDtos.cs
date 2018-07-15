using System;

namespace Sirius.Models.Dtos
{
    public class ItemDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double MinimumLimit { get; set; }
    }
}
