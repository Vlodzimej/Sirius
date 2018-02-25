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

        public User GetUserByLogin(string login)
        {
            var result = GetAllUsers().Where(x => x.Login == login).FirstOrDefault();
            return result;
        }
        public bool CheckUserById(Guid id)
        {
            if (unitOfWork.UserRepository.GetByID(id) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateUser(string login, string password)
        {
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
        }

        public void UpdateUser(User user)
        {
            var _user = unitOfWork.UserRepository.GetByID(user.Id);
            // Проверка был ли изменён пароль
            if(user.Password != null || user.Password != "")
            {
                string hashPassword = HashHelper.CalculateMD5Hash(user.Password);
                if(hashPassword != _user.Password)
                {
                    _user.Password = hashPassword;
                }
            }

            if(user.Login != null || user.Login != "")
            {
                if(user.Login != _user.Login)
                {
                    _user.Login = user.Login;
                }
            }

            unitOfWork.UserRepository.Update(_user);
            unitOfWork.Save();
        }

        public void DeleteUser(Guid id)
        {
            var user = unitOfWork.UserRepository.GetByID(id);
            unitOfWork.UserRepository.Delete(user);
            unitOfWork.Save();
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
