using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sirius.Models;
using Sirius.DAL;
using System.Web;
using Newtonsoft.Json;

namespace Sirius.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: api/User
        [HttpGet]
        public IEnumerable<User> Get()
        {
            var result = unitOfWork.UserRepository.Get();
            return result;
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(Guid id)
        {
            var user = unitOfWork.UserRepository.GetByID(id);
            string result = JsonConvert.SerializeObject(user).Replace(@"\", "");
            return result;
        }
        
        // POST: api/User
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
