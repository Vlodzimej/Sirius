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
        /// Получение категории по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Category GetCategoryById(Guid id);

        /// <summary>
        /// Получения списка категорий
        /// </summary>
        /// <returns></returns>
        IEnumerable<Category> GetAllCategory();

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteCategoryById(Guid id);

        /// <summary>
        /// Добавить новую категорию
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Category AddCategory(Category category);

        /// <summary>
        /// Обновить категорию
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        Category UpdateCategory(Guid categoryId, Category category);
    }
}
