using System;

namespace Sirius.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UnitId { get; set; }

        public Item(string name, Guid unitId)
        {
            Id = Guid.NewGuid();
            Name = name;
            UnitId = unitId;
        }
    }
}
