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
        // private SiriusContext context;

        public UserRepository(SiriusContext context) : base(context)
        { }

        public User GetByUsername(string username)
        {
            return _siriusContext.Users.Where(u => u.Username == username).FirstOrDefault();
        }

        public bool CheckUsername(string username)
        {
            return _siriusContext.Users.Any(u => u.Username == username);
        }
    }
}