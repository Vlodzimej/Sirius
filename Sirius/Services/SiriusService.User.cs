using System;
using System.Collections.Generic;
using System.Linq;
using Sirius.Models;
using Sirius.Helpers;
using Sirius.DAL;
using System.Threading.Tasks;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        /// <summary>
        /// Аутентификация пользователей
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = unitOfWork.UserRepository.GetByUsername(username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        /// <summary>
        /// Получения пользователя по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetById(Guid id)
        {
            return unitOfWork.UserRepository.GetByID(id);
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User CreateUser(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (unitOfWork.UserRepository.CheckUsername(user.Username)) {
                throw new AppException("Username " + user.Username + " is already taken");
            }
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            unitOfWork.UserRepository.Insert(user);
            unitOfWork.Save();

            return user;
        }

        /// <summary>
        /// Обновление данных пользователя
        /// </summary>
        /// <param name="userParam"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Update(User userParam, string password = null)
        {
            var user = unitOfWork.UserRepository.GetByID(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (unitOfWork.UserRepository.CheckUsername(user.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            unitOfWork.UserRepository.Update(user);
            unitOfWork.Save();
            user = unitOfWork.UserRepository.GetByID(user.Id);
            return user;

        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            var user = unitOfWork.UserRepository.GetByID(id);
            if (user != null)
            {
                unitOfWork.UserRepository.Delete(user);
                unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Создание хэша пароля
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Верификация хэша пароля
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedHash"></param>
        /// <param name="storedSalt"></param>
        /// <returns></returns>
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Получение пользователя по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(Guid id)
        {
            return unitOfWork.UserRepository.GetByID(id);
        }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsers()
        {
            return unitOfWork.UserRepository.Get();
        }
    }
}
