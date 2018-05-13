using Sirius.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sirius.DAL.Repository.Contract
{
    interface ISettingRepository
    {
        /// <summary>
        /// Удаление данных таблиц
        /// </summary>
        void DatabaseDropTables();

        /// <summary>
        /// Добавление первичных данных в базу данных
        /// </summary>
        /// <param name="invoiceTypes"></param>
        /// <param name="dimensions"></param>
        /// <param name="categories"></param>
        /// <param name="vendors"></param>
        /// <param name="items"></param>
        /// <param name="settings"></param>
        void DatabaseFill(
            List<InvoiceType> invoiceTypes,
            List<Dimension> dimensions,
            List<Category> categories,
            List<Vendor> vendors,
            List<Item> items,
            List<Setting> settings);
    }
}