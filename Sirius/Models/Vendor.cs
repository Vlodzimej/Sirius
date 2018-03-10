using System;

namespace Sirius.Models
{
    /// <summary>
    /// Поставщики
    /// </summary>
    public class Vendor
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Contact { get; set; }
    }
}
