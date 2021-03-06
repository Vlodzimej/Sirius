﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sirius.Models;
using Sirius.DAL;
using Sirius.Services;
using Sirius.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.Extensions.Options;
using Sirius.Models.Dtos;
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
        private SiriusService _siriusService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserController(IMapper mapper, IOptions<AppSettings> appSettings, UnitOfWork unitOfWork)
        {
            _siriusService = new SiriusService(unitOfWork);
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            var user = _siriusService.Authenticate(userDto.Username, userDto.Password);
            var role = _siriusService.GetRoleById(user.RoleId.Value);

            if (user == null)
                //return Unauthorized();
                return StatusCode(401, "Неверный логин или пароль.");

            if (user.IsConfirmed == false)
                return StatusCode(401, "Пользователь не подтверждён.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role?.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                user.Id,
                user.Username,
                user.FirstName,
                user.LastName,
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
                _siriusService.CreateUser(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("amount")]
        public IActionResult GetUserAmount()
        {
            return Ok(_siriusService.GetUserAmount());
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _siriusService.GetAllUsers();
            var userDtos = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = _siriusService.GetById(id);
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
                _siriusService.Update(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(Guid id)
        {
            _siriusService.Delete(id);
            return Ok();
        }

        [HttpPut("status/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult ChangeStatus(Guid id, [FromQuery]bool isConfirmed)
        {
            if (_siriusService.ChangeUserStatus(id, isConfirmed))
            {
                return StatusCode(200, "Статус пользователя изменён.");
            }
            return StatusCode(400, "Пользователь не найден.");
        }

        [HttpGet("checkadmin/{id}")]
        public IActionResult CheckAdmin(Guid id)
        {
            try
            {
                bool isAdmin = _siriusService.CheckAdminByUserId(id);
                return StatusCode(200, isAdmin);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
