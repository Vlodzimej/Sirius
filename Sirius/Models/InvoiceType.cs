using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public short Factor { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
    }
}
