using System;

namespace Sirius.Models
{
    /// <summary>
    /// Поставщик
    /// </summary>
    public class Vendor
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Контакты
        /// </summary>
        public string Contact { get; set; }
    }
}
