using System;
using Sirius.Models;

namespace Sirius.DAL
{
    public class UnitOfWork : IDisposable
    {
        private SiriusContext context = new SiriusContext();

        private GenericRepository<Item> itemRepository;
        public GenericRepository<Item> ItemRepository
        {
            get
            {
                return itemRepository ?? new GenericRepository<Item>(context);
            }
        }

        private UserRepository userRepository;
        public UserRepository UserRepository
        {
            get
            {
                return userRepository ?? new UserRepository(context);
            }
        }

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