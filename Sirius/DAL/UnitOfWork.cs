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
        private UserRepository userRepository;
        public UserRepository UserRepository => userRepository ?? new UserRepository(context);

        private GenericRepository<Dimension> dimensionRepository;
        public GenericRepository<Dimension> DimensionRepository => dimensionRepository ?? new GenericRepository<Dimension>(context);

        private GenericRepository<Category> categoryRepository;
        public GenericRepository<Category> CategoryRepository => categoryRepository ?? new GenericRepository<Category>(context);

        private GenericRepository<Vendor> vendorRepository;
        public GenericRepository<Vendor> VendorRepository => vendorRepository ?? new GenericRepository<Vendor>(context);

        private GenericRepository<Item> itemRepository;
        public GenericRepository<Item> ItemRepository => itemRepository ?? new GenericRepository<Item>(context);

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