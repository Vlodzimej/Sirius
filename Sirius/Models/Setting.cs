using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Models
{
    /// <summary>
    /// Класс внутренних настроек 
    /// </summary>
    public class Setting
    {
        public Guid Id { get; set; }
        // Ключ
        public string Name { get; set; }
        // Значение
        public string Value { get; set; }
    }
}
