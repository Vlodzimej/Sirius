using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Services
{
    public partial class SiriusService : ISiriusService
    {
        public Role GetRoleById(Guid roleId)
        {
            return _unitOfWork.RoleRepository.GetRoleById(roleId);
        }

    }
}
