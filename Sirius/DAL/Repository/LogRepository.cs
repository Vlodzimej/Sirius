using System;
using System.Collections.Generic;
using System.Linq;
using Sirius.DAL.Repository.Contract;
using Sirius.Models;
using System.Linq.Expressions;

namespace Sirius.DAL
{
    public class LogRepository : GenericRepository<Log>
    {
        public LogRepository(SiriusContext _siriusContext) : base(_siriusContext)
        { }
    }
}