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
        /// Получить поставщика по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Vendor GetVendorById(Guid id);

        /// <summary>
        /// Получить список всех поставщиков
        /// </summary>
        /// <returns></returns>
        IEnumerable<Vendor> GetAllVendors();

        /// <summary>
        /// Удалить поставщика
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteVendorById(Guid id);

        /// <summary>
        /// Добавить нового поставщика
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        Vendor AddVendor(Vendor vendor);

        /// <summary>
        /// Обновить данные поставщика
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="vendor"></param>
        /// <returns></returns>
        Vendor UpdateVendor(Guid vendorId, Vendor vendor);
    }
}
