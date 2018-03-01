using System;
using System.Collections.Generic;
using System.Linq;
using Sirius.Models;
using Sirius.Helpers;
using Sirius.DAL;
using System.Threading.Tasks;

namespace Sirius.BLL
{
    public partial class SiriusService : ISiriusService
    {
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

        public User GetById(Guid id)
        {
            return unitOfWork.UserRepository.GetByID(id);
        }

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

        public void Update(User userParam, string password = null)
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
        }

        public void Delete(Guid id)
        {
            var user = unitOfWork.UserRepository.GetByID(id);
            if (user != null)
            {
                unitOfWork.UserRepository.Delete(user);
                unitOfWork.Save();
            }
        }

        // private helper methods

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
        /// <returns>Пользователь</returns>
        public User GetUserById(Guid id)
        {
            return unitOfWork.UserRepository.GetByID(id);
        }

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns>Cписок всех пользователей</returns>
        public IEnumerable<User> GetAllUsers()
        {
            return unitOfWork.UserRepository.Get();
        }
        /*


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
        /// Проверка существования пользователя по логину и паролю
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool CheckUserByLoginAndPassword(string login, string password)
        {
            bool result = false;
            var user = GetAllUsers().Where(x => x.Login == login).FirstOrDefault();
            if(user != null)
            {
                if(user.Password == HashHelper.CalculateMD5Hash(password))
                {
                    result = true;
                }
            }

            return result;
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

        public Guid? Login(UserContract user)
        {
            Guid? result = null;
            if(CheckUserByLoginAndPassword(user.login, user.password))
            {
                result = GetUserByLogin(user.login).Id;
            }
            return result;
        }*/





    }
}
