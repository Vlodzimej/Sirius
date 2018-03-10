using Sirius.Models;
using Sirius.Models.Dtos;
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
        public ItemDetailDto GetItemById(Guid id)
        {
            var item = unitOfWork.ItemRepository.GetByID(id);
            return item;
        }

        /// <summary>
        /// Получить список всех предметов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemDto> GetAllItems()
        {
            var items = unitOfWork.ItemRepository.GetAll();
            return items;
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
        public ItemDetailDto AddItem(ItemSaveDto savingItem)
        {
            var item = new Item()
            {
                Id = Guid.NewGuid(),
                Name = savingItem.Name,
                DimensionId = savingItem.DimensionId,
                CategoryId = savingItem.CategoryId,
                VendorId = savingItem.VendorId
            };

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
        public ItemDetailDto UpdateItem(Guid itemId, Item item)
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
