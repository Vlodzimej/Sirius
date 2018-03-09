using Sirius.Models;
using System;
using System.Collections.Generic;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Получить поставщика по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Vendor GetVendorById(Guid id)
        {
            return unitOfWork.VendorRepository.GetByID(id);
        }

        /// <summary>
        /// Получить список всех поставщиков
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Vendor> GetAllVendors()
        {
            return unitOfWork.VendorRepository.Get();
        }

        /// <summary>
        /// Удалить поставщика
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteVendorById(Guid id)
        {
            var vendor = unitOfWork.VendorRepository.GetByID(id);
            if (vendor != null)
            {
                unitOfWork.CategoryRepository.Delete(vendor);
                unitOfWork.Save();
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

            unitOfWork.VendorRepository.Insert(vendor);
            unitOfWork.Save();

            return unitOfWork.VendorRepository.GetByID(vendor.Id) ?? null;
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
                unitOfWork.VendorRepository.Update(vendor);
                unitOfWork.Save();
                return unitOfWork.VendorRepository.GetByID(vendorId);
            }
            return null;
        }
    }
}
