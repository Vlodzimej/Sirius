using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Services
{
    partial interface ISiriusService
    {
        /// <summary>
        /// Получение единицы измерения по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Dimension GetDimensionById(Guid id);

        /// <summary>
        /// Получение всех единиц измерения
        /// </summary>
        /// <returns></returns>
        IEnumerable<Dimension> GetAllDimension();

        /// <summary>
        /// Удаление единицы измерения по идентификатору
        /// </summary>
        /// <param name="id"></param>
        void DeleteDimensionById(Guid id);

        /// <summary>
        /// Добавление новой единицы измерения
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        Dimension AddDimension(Dimension dimension);

        /// <summary>
        /// Обновление данных единицы измерения
        /// </summary>
        /// <param name="dimensionId"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        Dimension UpdateDimension(Guid dimensionId, Dimension dimension);
    }
}
