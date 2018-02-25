using System;
using System.Collections.Generic;
using System.Linq;
using Sirius.Models;
using Sirius.Helpers;

namespace Sirius.BLL
{
    public partial class SiriusService
    {
        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns>Cписок всех пользователей</returns>
        public IEnumerable<User> GetAllUsers()
        {
            return unitOfWork.UserRepository.Get();
        }
        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Пользователь</returns>
        public User GetUserById(Guid id)
        {
            return unitOfWork.UserRepository.GetByID(id);
        }

        /// <summary>
        /// Получение пользователя по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Пользователь</returns>
        public User GetUserByLogin(string login)
        {
            var result = GetAllUsers().Where(x => x.Login == login).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Проверка существования пользователя по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
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

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public void CreateUser(UserContract newUser)
        {
            string passwordHash = HashHelper.CalculateMD5Hash(newUser.password);

            User user = new User
            {
                Id = Guid.NewGuid(),
                Login = newUser.login,
                StartDate = DateTime.Now,
                FinishDate = DateTime.MaxValue,
                Password = passwordHash
            };

            unitOfWork.UserRepository.Insert(user);
            unitOfWork.Save();
        }

        /// <summary>
        /// Изменение пользовательской информации
        /// </summary>
        /// <param name="user">Объект пользователя</param>
        public void UpdateUser(User user)
        {
            var _user = unitOfWork.UserRepository.GetByID(user.Id);
            // Проверка был ли изменён пароль
            if (user.Password != null || user.Password != "")
            {
                string hashPassword = HashHelper.CalculateMD5Hash(user.Password);
                if (hashPassword != _user.Password)
                {
                    _user.Password = hashPassword;
                }
            }

            if (user.Login != null || user.Login != "")
            {
                if (user.Login != _user.Login)
                {
                    _user.Login = user.Login;
                }
            }

            unitOfWork.UserRepository.Update(_user);
            unitOfWork.Save();
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUser(Guid id)
        {
            var user = unitOfWork.UserRepository.GetByID(id);
            unitOfWork.UserRepository.Delete(user);
            unitOfWork.Save();
        }
    }
}
