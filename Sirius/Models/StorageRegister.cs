using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Models
{
    public class StorageRegister
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Идентификатор наименования
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public Item Item { get; set; }
    }
}
