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

        // GET: api/Users
        public IActionResult Get()
        {
            var users = siriusService.GetAllUsers();
            return new ObjectResult(users);
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(Guid id)
        {
            var user = siriusService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }

        /// <summary>
        /// Добавление пользователя
        /// POST: api/User 
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] UserContract newUser)
        {
            User user = null;
            // Отправлены ли данные нового пользователя
            if(newUser == null)
            {
                return BadRequest();
            }
            // Проверка заполнения полей логина и пароля
            if (newUser.login != null && newUser.password != null)
            {
                siriusService.CreateUser(newUser);
                user = siriusService.GetUserByLogin(newUser.login);
            }
            // Если пользователь не добавлен - возвращаем 415
            if (user == null)
            {
                return BadRequest();
            }
            // Обнуление пароля
            user.Password = null;
            // Возвращение положительного ответа с данными пользователя
            var result = CreatedAtRoute("Get", new { id = user.Id }, user);
            return result;
        }
        
        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]User user)
        {
            if (user == null || user.Id != id)
            {
                return BadRequest();
            }

            if (!siriusService.CheckUserById(id))
            {
                return NotFound();
            }

            siriusService.UpdateUser(user);

            return new NoContentResult();
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!siriusService.CheckUserById(id))
            {
                return NotFound();
            }
             siriusService.DeleteUser(id);
            return new NoContentResult();
        }
    }
}
