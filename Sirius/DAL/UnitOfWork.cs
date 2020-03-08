using System;
using Sirius.DAL.Repository;
using Sirius.Models;

namespace Sirius.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SiriusContext _siriusContext;

        /// <summary>
        /// Репозиторий "Пользователь"
        /// </summary>
        public UserRepository UserRepository => userRepository ?? new UserRepository(_siriusContext);
        private UserRepository userRepository;


        /// <summary>
        /// Репозиторий "Единица измерения"
        /// </summary>
        public GenericRepository<Dimension> DimensionRepository => dimensionRepository ?? new GenericRepository<Dimension>(_siriusContext);
        private GenericRepository<Dimension> dimensionRepository;

        /// <summary>
        /// Репозиторий "Категория"
        /// </summary>
        public GenericRepository<Category> CategoryRepository => categoryRepository ?? new GenericRepository<Category>(_siriusContext);
        private GenericRepository<Category> categoryRepository;

        /// <summary>
        /// Репозиторий "Поставщик"
        /// </summary>
        public GenericRepository<Vendor> VendorRepository => vendorRepository ?? new GenericRepository<Vendor>(_siriusContext);
        private GenericRepository<Vendor> vendorRepository;

        /// <summary>
        /// Репозиторий "Предмет"
        /// </summary>
        public ItemRepository ItemRepository => itemRepository ?? new ItemRepository(_siriusContext);
        private ItemRepository itemRepository;


        /// <summary>
        /// Репозиторий "Регистр"
        /// </summary>
        public RegisterRepository RegisterRepository => registerRepository ?? new RegisterRepository(_siriusContext);
        private RegisterRepository registerRepository;

        /// <summary>
        /// Репозиторий "Накладная"
        /// </summary>
        public InvoiceRepository InvoiceRepository => invoiceRepository ?? new InvoiceRepository(_siriusContext);
        private InvoiceRepository invoiceRepository;

        /// <summary>
        /// Репозиторий "Настройки"
        /// </summary>
        public SettingRepository SettingRepository => settingRepository ?? new SettingRepository(_siriusContext);
        private SettingRepository settingRepository;

        /// <summary>
        /// Репозиторий "Регистр накопления"
        /// </summary>
        public StorageRegisterRepository StorageRegisterRepository => storageRegisterRepository ?? new StorageRegisterRepository(_siriusContext);
        private StorageRegisterRepository storageRegisterRepository;

        public RoleRepository RoleRepository => roleRepository ?? new RoleRepository(_siriusContext);
        private RoleRepository roleRepository;

        /// <summary>
        /// Репозиторий "Логи"
        /// </summary>
        public LogRepository LogRepository => logRepository ?? new LogRepository(_siriusContext);
        private LogRepository logRepository;


        public UnitOfWork(SiriusContext siriusContext)
        {
            _siriusContext = siriusContext;
        }

        public void Save()
        {
            _siriusContext.SaveChanges();
        }

        #region Disposed
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _siriusContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}