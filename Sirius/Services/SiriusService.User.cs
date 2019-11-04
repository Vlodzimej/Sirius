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

            currentUser = _unitOfWork.UserRepository.GetByUsername(username);

            // check if username exists
            if (currentUser == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, currentUser.PasswordHash, currentUser.PasswordSalt))
                return null;

            // authentication successful
            return currentUser;
        }

        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(Guid id)
        {
            return _unitOfWork.UserRepository.GetByID(id);
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
                throw new AppException("Требуется ввести пароль.");

            if (_unitOfWork.UserRepository.CheckUsername(user.Username))
            {
                throw new AppException("Пользователь " + user.Username + " существует.");
            }
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            user.StartDate = DateConverter.ConvertToRTS(DateTime.UtcNow.ToLocalTime());


            // Учетная запись первого зарегистрированного пользователя должна быть подтверждена
            if (GetUserAmount() == 0)
            {
                user.IsConfirmed = true;
            }

            _unitOfWork.UserRepository.Insert(user);
            _unitOfWork.Save();

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
            var user = _unitOfWork.UserRepository.GetByID(userParam.Id);

            if (user == null)
                throw new AppException("Пользователь не найден.");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (_unitOfWork.UserRepository.CheckUsername(user.Username))
                    throw new AppException("Пользователь " + userParam.Username + " существует.");
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

            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();
            user = _unitOfWork.UserRepository.GetByID(user.Id);
            return user;

        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            var user = _unitOfWork.UserRepository.GetByID(id);
            if (user != null)
            {
                _unitOfWork.UserRepository.Delete(user);
                _unitOfWork.Save();
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
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Пароль не может быть пустым или содержать пробелы.", "password");

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
        /// Получение пользователя по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetById(Guid id)
        {
            return _unitOfWork.UserRepository.GetByID(id);
        }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsers()
        {
            return _unitOfWork.UserRepository.Get();
        }

        /// <summary>
        /// Изменить статус пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isConfirmed"></param>
        /// <returns></returns>
        public bool ChangeUserStatus(Guid userId, bool isConfirmed = false)
        {
            var user = GetUserById(userId);
            if (user != null)
            {
                user.IsConfirmed = isConfirmed;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Получение кол-ва зарегистрированных пользователей
        /// </summary>
        /// <returns></returns>
        public int GetUserAmount()
        {
            return _unitOfWork.UserRepository.GetUserAmount();
        }

        /// <summary>
        /// Проверка есть ли у пользователя права администратора
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CheckAdminByUserId(Guid userId)
        {
            return _unitOfWork.UserRepository.GetByID(userId).RoleId == DefaultValues.Roles.Admin.Id;
        }
    }
}
