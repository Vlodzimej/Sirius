using System;
using System.Collections.Generic;

namespace Sirius.Models
{
    /// <summary>
    /// Накладные
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Name { get;set; }

        /// <summary>
        /// Идентификатор поставщика
        /// </summary>
        public Guid VendorId { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public Vendor Vendor { get; set; }

        /// <summary>
        /// Идентификатор пользователя-автора
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Пользователь-автор
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Список регистров
        /// </summary>
        public IEnumerable<Register> Registers { get; set; }

        /// <summary>
        /// Признак проведённой накладной
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// Признак временной накладной
        /// </summary>
        public bool IsTemporary { get; set; }

        /// <summary>
        /// Дата создания 
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Идентификатор типа накладной
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Фактор
        /// </summary>
        public double Factor { get; set; }
    }
}
