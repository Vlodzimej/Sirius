using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirius.DAL;
using Sirius.Models;
using Sirius.Helpers;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        public void DatabaseReset()
        {
            // Создание списка категорий
            var categoryPrototypes = new List<Category>();
            var category = new Category() {
                Id = Guid.Parse("B5E7EACE-1AED-4173-B383-32C7F9643F44"),
                Name = "Расходные материалы"
            };
            categoryPrototypes.Add(category);

            // Создание списка единиц измерения
            var dimensionPrototypes = new List<Dimension>();

            var dimension = new Dimension()
            {
                Id = Guid.Parse("3BF5C95A-2002-457F-8299-86BF25839480"),
                Name = "шт."
            };
            dimensionPrototypes.Add(dimension);
            dimension = new Dimension()
            {
                Id = Guid.Parse("E35BB3F7-E944-4E87-BC59-F4B5A1F5C219"),
                Name = "пар."
            };
            dimensionPrototypes.Add(dimension);
            dimension = new Dimension()
            {
                Id = Guid.Parse("6358D37D-FC67-48FE-85EA-DB404106BD99"),
                Name = "л."
            };
            dimensionPrototypes.Add(dimension);
            dimension = new Dimension()
            {
                Id = Guid.Parse("41B62CF2-B534-4619-BD22-68B3756FFC04"),
                Name = "мл."
            };
            dimensionPrototypes.Add(dimension);

            // Создание списка поставщиков
            var vendorPrototypes = new List<Vendor>();
            var vendor = new Vendor()
            {
                Id = DefaultValues.Vendor.Primary.Id,
                Name = "Основной поставщик",
                Address = "г. Калуга",
                Contact = "тел.: 8(4842)77-77-77"
            };
            vendorPrototypes.Add(vendor);

            // Создание списка наименований
            var itemPrototypes = new List<Item>();
            var item = new Item()
            {
                Id = Guid.Parse("C4C00A31-8661-4C1B-A699-651AF03097FE"),
                Name = "Перчатки резиновые",
                DimensionId = dimensionPrototypes.Where(d => d.Id == Guid.Parse("E35BB3F7-E944-4E87-BC59-F4B5A1F5C219")).FirstOrDefault().Id,
                CategoryId = categoryPrototypes.FirstOrDefault().Id
            };
            itemPrototypes.Add(item);


            _unitOfWork.SettingRepository.DatabaseDropTables();
            _unitOfWork.Save();

            _unitOfWork.SettingRepository.DatabaseFill(dimensionPrototypes, categoryPrototypes, vendorPrototypes, itemPrototypes);
            _unitOfWork.Save();
        }

    }
}
