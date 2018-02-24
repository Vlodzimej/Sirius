using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Sirius.DAL;

namespace Sirius.Models
{
    public class UserRepository : IUserRepository
    {
        private SiriusContext context;

        private static ConcurrentDictionary<string, User> _users =
              new ConcurrentDictionary<string, User>();

        public UserRepository(SiriusContext context)
        {
            this.context = context;
        }

        public IEnumerable<User> GetAll()
        {
            return _users.Values;
        }

        public void Add(User user)
        {
            user.Id = Guid.NewGuid();
            _users[user.Id.ToString()] = user;
        }

        public User Find(Guid id)
        {
            User user;
            _users.TryGetValue(id.ToString(), out user);
            return user;
        }

        public User Remove(Guid id)
        {
            User user;
            _users.TryRemove(id.ToString(), out user);
            return user;
        }

        public void Update(User user)
        {
            _users[user.Id.ToString()] = user;
        }
    }
}