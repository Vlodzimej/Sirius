using Sirius.Models;
using Sirius.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Sirius.Helpers;
using Sirius.Extends.Filters;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получить предмет по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object GetItemById(Guid id)
        {
            var item = _unitOfWork.ItemRepository.GetById(id);
            return item;
        }

        /// <summary>
        /// Получить список всех предметов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetAllItems()
        {
            var items = _unitOfWork.ItemRepository.GetAll();
            return items;
        }

        /// <summary>
        /// Получить список всех предметов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetItemsByFilter(ItemFilter filter)
        {
            if (filter != null)
            {
                var items = _unitOfWork.ItemRepository.GetAll(x => 
                    (filter.CategoryId != Guid.Empty ? x.CategoryId == filter.CategoryId : true) &&
                    (filter.ItemId != Guid.Empty ? x.Id == filter.ItemId : true));
                return items;
            }
            return null;
        }



        /// <summary>
        /// Удалить предмет
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteItemById(Guid id)
        {
            var item = _unitOfWork.ItemRepository.GetByID(id);
            var registers = _unitOfWork.RegisterRepository.GetFixedRegisters();
            // Если какой-либо регистр проведённой накладной ссылается на предмет, то удалять предмет запрещено
            if (item != null)
            {
                if (registers.FirstOrDefault(x => x.ItemId == item.Id) == null)
                {
                    _unitOfWork.ItemRepository.Delete(item);
                    _unitOfWork.Save();
                    return item.Id.ToString();
                } else
                {
                    return "Наименование невозможно удалить, так как на него ссылаются проведённые документы!";
                }
            } 
            return "Наименование не найдено.";
        }

        /// <summary>
        /// Добавить новый предмет
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public object AddItem(ItemSaveDto savingItem)
        {
            var item = new Item()
            {
                Id = Guid.NewGuid(),
                Name = savingItem.Name,
                DimensionId = savingItem.DimensionId,
                CategoryId = savingItem.CategoryId,
                IsCountless = savingItem.IsCountless,
                MinimumLimit = savingItem.MinimumLimit
            };

            _unitOfWork.ItemRepository.Insert(item);
            _unitOfWork.Save();

            return _unitOfWork.ItemRepository.GetById(item.Id);
        }

        /// <summary>
        /// Обновить информацию о предмете
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public object UpdateItem(Guid itemId, Item item)
        {
            if (itemId == item.Id)
            {
                _unitOfWork.ItemRepository.Update(item);
                _unitOfWork.Save();
                return _unitOfWork.ItemRepository.GetById(itemId);
            }
            return null;
        }

         
    }
}
