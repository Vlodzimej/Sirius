using Sirius.Extends.Filters;
using Sirius.Models;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Services
{
    partial interface ISiriusService
    {
        /// <summary>
        /// Получить предмет по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        object GetItemById(Guid id);

        /// <summary>
        /// Получить список всех предметов
        /// </summary>
        /// <returns></returns>
        IEnumerable<object> GetAllItems();

        /// <summary>
        /// Получить список всех предметов
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<object> GetItemsByFilter(ItemFilter filter);

        /// <summary>
        /// Удалить предмет
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteItemById(Guid id);

        /// <summary>
        /// Добавить новый предмет
        /// </summary>
        /// <param name="savingItem"></param>
        /// <returns></returns>
        object AddItem(ItemSaveDto savingItem);

        /// <summary>
        /// Обновить информацию о предмете
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        object UpdateItem(Guid itemId, Item item);
    }
}
