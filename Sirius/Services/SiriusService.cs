using Sirius.DAL;
using System;
using Sirius.Models;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        private UnitOfWork unitOfWork;
        private User currentUser;

        public SiriusService(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        #region IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    unitOfWork.Dispose();
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
