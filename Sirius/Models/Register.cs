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
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Идентификатор наименования
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Идентификатор накладной
        /// </summary>
        public Guid InvoiceId { get; set; }

        /// <summary>
        /// Накладная
        /// </summary>
        public Invoice Invoice { get; set; }
    }
}
