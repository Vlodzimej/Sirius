using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Models
{
    /// <summary>
    /// Накладные
    /// </summary>
    public class Invoice
    {
        // Идентификатор
        public Guid Id { get; set; }
        // Заголовок
        public string Name { get;set; }
        // Идентификатор поставщика
        public Guid VendorId { get; set; }
        // Поставщик
        public virtual Vendor Vendor { get; set; }
        // Идентификатор пользователя-автора
        public Guid UserId { get; set; }
        // Пользователь-автор
        public virtual User User { get; set; }
        // Список регистров
        public virtual List<Register> Registers { get; set; }
        // Признак проведённой накладной
        public bool IsRecorded { get; set; }
        // Признак временной накладной
        public bool IsTemporary { get; set; }
        // Дата создания 
        public DateTime CreateDate { get; set; }
    }
}
