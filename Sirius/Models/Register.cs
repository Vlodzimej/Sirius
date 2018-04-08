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
        // Идентификатор
        public Guid Id { get; set; }
        // Стоимость
        public decimal Cost { get; set; }
        // Количество
        public double Amount { get; set; }
        // Идентификатор наименования
        public Guid ItemId { get; set; }
        // Наименование
        public Item Item { get; set; }
        // Идентификатор накладной
        public Guid InvoiceId { get; set; }
        //
        public Invoice Invoice { get; set; }
    }
}
