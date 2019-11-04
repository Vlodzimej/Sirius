using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.DAL.Repository.Contracts
{
    interface IRoleRepository
    {
        Role GetRoleById(Guid roleId);
    }
}
