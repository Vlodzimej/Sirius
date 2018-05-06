using System;

namespace Sirius.Models
{
    /// <summary>
    /// Класс внутренних настроек 
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Идентификатор настройки
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Название настройки
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Значение настройки
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Тип настройки
        /// </summary>
        public Guid TypeId { get; set; }
        /// <summary>
        /// Псевдоним
        /// </summary>
        public string Alias { get; set; }
    }
}
