using System;
using System.Collections.Generic;
using Sirius.Models;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получение категории по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category GetCategoryById(Guid id)
        {
            return unitOfWork.CategoryRepository.GetByID(id);
        }

        /// <summary>
        /// Получения списка категорий
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> GetAllCategory()
        {
            return unitOfWork.CategoryRepository.Get();
        }

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteCategoryById(Guid id)
        {
            var category = unitOfWork.CategoryRepository.GetByID(id);
            if (category != null)
            {
                unitOfWork.CategoryRepository.Delete(category);
                unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавить новую категорию
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Category AddCategory(Category category)
        {
            category.Id = Guid.NewGuid();

            unitOfWork.CategoryRepository.Insert(category);
            unitOfWork.Save();

            return unitOfWork.CategoryRepository.GetByID(category.Id) ?? null;
        }

        /// <summary>
        /// Обновить категорию
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public Category UpdateCategory(Guid categoryId, Category category)
        {
            if (categoryId == category.Id)
            {
                unitOfWork.CategoryRepository.Update(category);
                unitOfWork.Save();
                return unitOfWork.CategoryRepository.GetByID(categoryId);
            }
            return null;
        }
    }
}
