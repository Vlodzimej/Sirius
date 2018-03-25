using Sirius.Models;
using System.Threading.Tasks;

namespace Sirius.DAL
{
    interface IUserRepository
    {
        User GetByUsername(string username);
        bool CheckUsername(string username);
    }
}