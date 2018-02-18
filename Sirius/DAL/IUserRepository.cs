using System;
using System.Collections.Generic;
using Sirius.Models;

namespace Sirius.DAL
{
    interface IUserRepository
    {
        void Add(User user);
        IEnumerable<User> GetAll();
        User Find(Guid id);
        User Remove(Guid id);
        void Update(User user);
    }
}