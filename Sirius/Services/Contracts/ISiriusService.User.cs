using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Services
{
    partial interface ISiriusService
    {
        /// <summary>
        /// Аутентификация пользователей
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User Authenticate(string username, string password);

        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetById(Guid id);

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User CreateUser(User user, string password);

        /// <summary>
        /// Обновление данных пользователя
        /// </summary>
        /// <param name="userParam"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User Update(User userParam, string password = null);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(Guid id);

        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetUserById(Guid id);

        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetAllUsers();

        /// <summary>
        /// Изменить статус пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isConfirmed"></param>
        /// <returns></returns>
        bool ChangeUserStatus(Guid userId, bool isConfirmed = false);

        /// <summary>
        /// Получение кол-ва зарегистрированных пользователей
        /// </summary>
        /// <returns></returns>
        int GetUserAmount();

        /// <summary>
        /// Проверка есть ли у пользователя права администратора
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool CheckAdminByUserId(Guid userId);
    }
}
