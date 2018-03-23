using System;
using Sirius.Models;

namespace Sirius.DAL
{
    public class UnitOfWork : IDisposable
    {
        private SiriusContext context = new SiriusContext();
        /*
        private GenericRepository<Item> itemRepository;
        public GenericRepository<Item> ItemRepository
        {
            get
            {
                return itemRepository ?? new GenericRepository<Item>(context);
            }
        }
        */
        /// <summary>
        /// Репозиторий "Пользователь"
        /// </summary>
        public UserRepository UserRepository => userRepository ?? new UserRepository(context);
        private UserRepository userRepository;


        /// <summary>
        /// Репозиторий "Единица измерения"
        /// </summary>
        public GenericRepository<Dimension> DimensionRepository => dimensionRepository ?? new GenericRepository<Dimension>(context);
        private GenericRepository<Dimension> dimensionRepository;

        /// <summary>
        /// Репозиторий "Категория"
        /// </summary>
        public GenericRepository<Category> CategoryRepository => categoryRepository ?? new GenericRepository<Category>(context);
        private GenericRepository<Category> categoryRepository;

        /// <summary>
        /// Репозиторий "Поставщик"
        /// </summary>
        public GenericRepository<Vendor> VendorRepository => vendorRepository ?? new GenericRepository<Vendor>(context);
        private GenericRepository<Vendor> vendorRepository;

        /// <summary>
        /// Репозиторий "Предмет"
        /// </summary>
        public ItemRepository ItemRepository => itemRepository ?? new ItemRepository(context);
        private ItemRepository itemRepository;


        /// <summary>
        /// Репозиторий "Регистр"
        /// </summary>
        public GenericRepository<Register> RegisterRepository => registerRepository ?? new GenericRepository<Register>(context);
        private GenericRepository<Register> registerRepository;

        /// <summary>
        /// Репозиторий "Накладная"
        /// </summary>
        public InvoiceRepository InvoiceRepository => invoiceRepository ?? new InvoiceRepository(context);
        private InvoiceRepository invoiceRepository;

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}