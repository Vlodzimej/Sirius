using System;
using Sirius.Models;
using Sirius.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.BLL
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получить единицу измерения по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Dimension</returns>
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
        /// <param name="id"></param>
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
        /// <param name="dimension"></param>
        public void AddDimension(Dimension dimension)
        {
            unitOfWork.DimensionRepository.Insert(dimension);
        }
    }
}
