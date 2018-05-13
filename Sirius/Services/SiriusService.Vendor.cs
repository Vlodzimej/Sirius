using Sirius.Models;
using System;
using System.Collections.Generic;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получить поставщика по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Vendor GetVendorById(Guid id)
        {
            return _unitOfWork.VendorRepository.GetByID(id);
        }

        /// <summary>
        /// Получить список всех поставщиков
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Vendor> GetAllVendors()
        {
            return _unitOfWork.VendorRepository.Get();
        }

        /// <summary>
        /// Удалить поставщика
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteVendorById(Guid id)
        {
            var vendor = _unitOfWork.VendorRepository.GetByID(id);
            if (vendor != null)
            {
                _unitOfWork.CategoryRepository.Delete(vendor);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавить нового поставщика
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        public Vendor AddVendor(Vendor vendor)
        {
            vendor.Id = Guid.NewGuid();

            _unitOfWork.VendorRepository.Insert(vendor);
            _unitOfWork.Save();

            return _unitOfWork.VendorRepository.GetByID(vendor.Id) ?? null;
        }

        /// <summary>
        /// Обновить данные поставщика
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="vendor"></param>
        /// <returns></returns>
        public Vendor UpdateVendor(Guid vendorId, Vendor vendor)
        {
            if (vendorId == vendor.Id)
            {
                _unitOfWork.VendorRepository.Update(vendor);
                _unitOfWork.Save();
                return _unitOfWork.VendorRepository.GetByID(vendorId);
            }
            return null;
        }
    }
}
