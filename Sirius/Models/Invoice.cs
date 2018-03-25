using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Models
{
    /// <summary>
    /// Накладные
    /// </summary>
    public class Invoice
    {
        // Уникальный идентификатор
        [Key]
        public Guid Id { get; set; }
        // Заголовок
        public string Name { get;set; }
        // Идентификатор поставщика
        public Guid VendorId { get; set; }
        // Поставщик
        public Vendor Vendor { get; set; }
        // Идентификатор пользователя-автора
        public Guid UserId { get; set; }
        // Пользователь-автор
        public User User { get; set; }
        // Список регистров
        public List<Register> Registers { get; set; }
        // Признак проведённой накладной
        public bool IsRecorded { get; set; }
        // Признак временной накладной
        public bool IsTemporary { get; set; }
        // Дата создания 
        public DateTime CreateDate { get; set; }
    }
}
