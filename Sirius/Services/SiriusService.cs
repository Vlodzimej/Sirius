using Sirius.DAL;
using System;
using Sirius.Models;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {

        private UnitOfWork _unitOfWork;
        //private SiriusContext _siriusContext;
        private User currentUser;

        public SiriusService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //_siriusContext = new SiriusContext();
        }

        #region IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
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
