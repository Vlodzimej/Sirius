using System;
using Sirius.Models;
using System.Collections.Generic;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получить единицу измерения по Id
        /// </summary>
        /// <param name="id">идентификатор единицы измерения</param>
        /// <returns>Единица измерения (Dimension)</returns>
        public Dimension GetDimensionById(Guid id)
        {
            return unitOfWork.DimensionRepository.GetByID(id);
        }

        /// <summary>
        /// Получение всех единиц измерения
        /// </summary>
        /// <returns>Cписок всех пользователей</returns>
        public IEnumerable<Dimension> GetAllDimension()
        {
            return unitOfWork.DimensionRepository.Get();
        }

        /// <summary>
        /// Удаление единицы измерения по Id
        /// </summary>
        /// <param name="id">идентификатор единицы измерения</param>
        public void DeleteDimensionById(Guid id)
        {
            var dimension = unitOfWork.DimensionRepository.GetByID(id);
            if (dimension != null)
            {
                unitOfWork.DimensionRepository.Delete(dimension);
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Добавление новой единицы измерения
        /// </summary>
        /// <param name="dimension">единица измерения</param>
        public Dimension AddDimension(Dimension dimension)
        {
            dimension.Id = Guid.NewGuid();

            unitOfWork.DimensionRepository.Insert(dimension);
            unitOfWork.Save();

            return unitOfWork.DimensionRepository.GetByID(dimension.Id) ?? null;
        }

        /// <summary>
        /// Обновление данных единицы измерения
        /// </summary>
        /// <param name="dimensionId">идентификатор единицы измерения</param>
        /// <param name="dimension">единица измерения</param>
        /// <returns>Обновленная единица измерения</returns>
        public Dimension UpdateDimension(Guid dimensionId, Dimension dimension)
        {
            if (dimensionId == dimension.Id)
            {
                unitOfWork.DimensionRepository.Update(dimension);
                unitOfWork.Save();
                return unitOfWork.DimensionRepository.GetByID(dimensionId);
            }
            return null;
        }
    }
}
