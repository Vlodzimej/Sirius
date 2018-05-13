using Sirius.Models;
using Sirius.DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Sirius.DAL.Repository.Contract;

namespace Sirius.Models
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SiriusContext context) : base(context)
        { }
           
        /// <summary>
        /// Получить учетную запись по логину
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetByUsername(string username)
        {
            return _siriusContext.Users.Where(u => u.Username == username).FirstOrDefault();
        }

        /// <summary>
        /// Проверить существование учетной записи по логину
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckUsername(string username)
        {
            return _siriusContext.Users.Any(u => u.Username == username);
        }

        /// <summary>
        /// Получить общее кол-во зарегистрированных пользователей
        /// </summary>
        /// <returns></returns>
        public int GetUserAmount()
        {
            return _siriusContext.Users.Count();
        }
    }
}