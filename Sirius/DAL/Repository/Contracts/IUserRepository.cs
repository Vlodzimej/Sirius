using Sirius.Models;
using System.Threading.Tasks;

namespace Sirius.DAL.Repository.Contract
{
    interface IUserRepository
    {
        /// <summary>
        /// Получить учетную запись по логину
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        User GetByUsername(string username);

        /// <summary>
        /// Проверить существование учетной записи по логину
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool CheckUsername(string username);

        /// <summary>
        /// Получить общее кол-во зарегистрированных пользователей
        /// </summary>
        /// <returns></returns>
        int GetUserAmount();
    }
}