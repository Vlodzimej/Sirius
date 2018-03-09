using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получить предмет по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Item GetItemById(Guid id)
        {
            return unitOfWork.ItemRepository.GetByID(id);
        }

        /// <summary>
        /// Получить список всех предметов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Item> GetAllItems()
        {
            return unitOfWork.ItemRepository.Get();
        }

        /// <summary>
        /// Удалить предмет
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteItemById(Guid id)
        {
            var item = unitOfWork.ItemRepository.GetByID(id);
            if (item != null)
            {
                unitOfWork.ItemRepository.Delete(item);
                unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавить новый предмет
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Item AddItem(Item item)
        {
            item.Id = Guid.NewGuid();

            unitOfWork.ItemRepository.Insert(item);
            unitOfWork.Save();

            return unitOfWork.ItemRepository.GetByID(item.Id) ?? null;
        }

        /// <summary>
        /// Обновить информацию о предмете
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public Item UpdateItem(Guid itemId, Item item)
        {
            if (itemId == item.Id)
            {
                unitOfWork.ItemRepository.Update(item);
                unitOfWork.Save();
                return unitOfWork.ItemRepository.GetByID(itemId);
            }
            return null;
        }
    }
}
