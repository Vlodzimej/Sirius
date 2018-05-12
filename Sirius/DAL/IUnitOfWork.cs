using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
