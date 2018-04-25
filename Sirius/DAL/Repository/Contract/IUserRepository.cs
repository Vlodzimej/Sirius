using Sirius.Models;
using System.Threading.Tasks;

namespace Sirius.DAL.Repository.Contract
{
    interface IUserRepository
    {
        User GetByUsername(string username);
        bool CheckUsername(string username);
    }
}