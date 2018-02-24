using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sirius.Models;
using Sirius.DAL;
using Sirius.BLL;
using System.Web;
using Newtonsoft.Json;

namespace Sirius.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private SiriusService siriusService = new SiriusService(new UnitOfWork());

        // GET: api/User
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return siriusService.GetAllUsers();
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public User Get(Guid id)
        {
            return siriusService.GetUserById(id);
        }
        
        // POST: api/User
        [HttpPost]
        public bool Post([FromQuery]string login, [FromQuery]string password)
        {
            return siriusService.CreateUser(login, password);
        }
        
        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put([FromBody]string value)
        {

        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            return siriusService.DeleteUser(id);
        }
    }
}
