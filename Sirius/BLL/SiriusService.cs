using Sirius.DAL;
using System;

namespace Sirius.BLL
{
    public partial class SiriusService : ISiriusService
    {
        private UnitOfWork unitOfWork;

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
