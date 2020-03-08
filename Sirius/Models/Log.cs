using System;

namespace Sirius.Models
{
    public class Log
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Содержание лога
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public string Action { get; set; }
    }
}