using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirius.Models;

namespace Sirius.Models
{
    public class DataBaseAccess
    {
        public void CreateDB()
        {
            using (SiriusContext db = new SiriusContext())
            {
                // создаем два объекта User
                User user1 = new User { Id = Guid.NewGuid(), Login = "vlodzi", Password = "111" };
                User user2 = new User { Id = Guid.NewGuid(), Login = "admin", Password = "admin" };

                // добавляем их в бд
                db.Users.Add(user1);
                db.Users.Add(user2);
                db.SaveChanges();

                // получаем объекты из бд и выводим на консоль
                /*
                var users = db.Users.ToList();
                Console.WriteLine("Users list:");
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
                }
                */
            }
            // Console.Read();
        }
    }
}
