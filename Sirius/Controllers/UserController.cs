using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sirius.Models;
using Sirius.DAL;
using Sirius.BLL;
using Sirius.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.Extensions.Options;
using Sirius.Dtos;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Sirius.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/user")]

    public class UserController : Controller
    {
        private SiriusService siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserController(IMapper mapper, IOptions<AppSettings> appSettings)
        {
            siriusService = new SiriusService(new UnitOfWork());
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            var user = siriusService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            try
            {
                // save 
                siriusService.CreateUser(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = siriusService.GetAllUsers();
            var userDtos = _mapper.Map<IEnumerable<User> , IEnumerable<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = siriusService.GetById(id);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]UserDto userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            try
            {
                // save 
                siriusService.Update(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            siriusService.Delete(id);
            return Ok();
        }


        /*
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
            if (newUser == null)
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserContract user)
        {
            Guid? userId;
            userId = siriusService.Login(user);

            if (userId == null)
            {
                return new BadRequestResult();
            }
            else
            {
                await Authenticate(user.login);
                return new JsonResult(userId);
            }
        }

        private async Task Authenticate(string login)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            var claimsPrincipal = new ClaimsPrincipal(id);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        */
    }
}
