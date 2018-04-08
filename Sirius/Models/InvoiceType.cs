using System;
using System.ComponentModel.DataAnnotations;

namespace Sirius.Models
{
    public class InvoiceType
    {
        /// <summary>
        /// Идентификатор типа накладной
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Фактор накладной
        /// </summary>
        public int Factor { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Алиас
        /// </summary>
        public string Alias { get; set; }
    }
}
