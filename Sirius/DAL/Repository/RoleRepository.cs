using Sirius.DAL.Repository.Contracts;
using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.DAL.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(SiriusContext _siriusContext) : base(_siriusContext)
        { }

        public Role GetRoleById(Guid roleId)
        {
            return _siriusContext.Roles.Where(role => role.Id == roleId).FirstOrDefault();
        }
    }
}