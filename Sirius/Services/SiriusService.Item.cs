using Sirius.Models;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Sirius.Helpers;

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
            var item = _unitOfWork.ItemRepository.GetByID(id);
            return item;
        }

        /// <summary>
        /// Получить список всех предметов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemDto> GetAllItems()
        {
            var items = _unitOfWork.ItemRepository.GetAll();
            return items;
        }

        /// <summary>
        /// Получить список всех предметов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ItemDto> GetItemsByFilter(MetaFilter filter)
        {
            if (filter != null)
            {
                var items = _unitOfWork.ItemRepository.GetAll(x => x.CategoryId == filter.categoryId);
                return items;
            }
            return null;
        }



        /// <summary>
        /// Удалить предмет
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteItemById(Guid id)
        {
            var item = _unitOfWork.ItemRepository.GetByID(id);
            if (item != null)
            {
                _unitOfWork.ItemRepository.Delete(item);
                _unitOfWork.Save();
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
                isCountless = savingItem.isCountless
            };

            _unitOfWork.ItemRepository.Insert(item);
            _unitOfWork.Save();

            return _unitOfWork.ItemRepository.GetByID(item.Id) ?? null;
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
                _unitOfWork.ItemRepository.Update(item);
                _unitOfWork.Save();
                return _unitOfWork.ItemRepository.GetByID(itemId);
            }
            return null;
        }

         
    }
}
