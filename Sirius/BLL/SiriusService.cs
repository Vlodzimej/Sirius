using Newtonsoft.Json;
using Sirius.DAL;
using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirius.Helpers;


namespace Sirius.BLL
{
    public class SiriusService : ISiriusService
    {
        private UnitOfWork unitOfWork;

        public SiriusService(UnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return unitOfWork.UserRepository.Get();
        }

        public User GetUserById(Guid id)
        {
            return unitOfWork.UserRepository.GetByID(id);
        }

        public bool CreateUser(string login, string password)
        {
            bool result = true;

            string passwordHash = HashHelper.CalculateMD5Hash(password);

            User user = new User
            {
                Id = Guid.NewGuid(),
                Login = login,
                StartDate = DateTime.Now,
                FinishDate = DateTime.MaxValue,
                Password = passwordHash
            };

            unitOfWork.UserRepository.Insert(user);
            unitOfWork.Save();

            return result;
        }

        public bool DeleteUser(Guid id)
        {
            var user = unitOfWork.UserRepository.GetByID(id);
            unitOfWork.UserRepository.Delete(user);
            unitOfWork.Save();
            return true;
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
