using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        /*
        public User(string login, string password)
        {
            Id = Guid.NewGuid();
            Login = login;
            Password = password;
            StartDate = DateTime.Now;
            FinishDate = DateTime.MaxValue;
        }*/
    }
}
