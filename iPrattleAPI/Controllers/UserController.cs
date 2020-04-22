using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iPrattle.Services;
using iPrattle.Services.Entities;
using iPrattleAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

namespace iPrattleAPI.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserSvc _userSvc;

        public UserController(IUserSvc userSvc, IConfiguration config) : base(config)
        {
            Log.Information("UserController Constructor");
            _userSvc = userSvc;
        }



		[HttpPost]
		[Route("~/login")]
		public IActionResult Login([FromBody]LoginRequest request)
        {
			try
			{
				var user = _userSvc.Login(request.Username, request.Password);
				if (user != null)
				{
					return Ok(user);
				}
				else
				{
					return Ok(null);
				}
			}
			catch(Exception ex)
			{
				Log.Error($"Failed while logging in user: {request.Username} : {ex.Message}");
			}
			return BadRequest();
        }


        [HttpPost("~/register")]
        public IActionResult Register([FromBody]User model)
        {
			try
			{
				Log.Information($"Creating new user for {JsonConvert.SerializeObject(model)}");

				var resultMessage = _userSvc.Register(model);
				return Ok(new
				{
					message = resultMessage
				});
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to register user, Name: {model.FirstName} {model.LastName}; Username: {model.Username} : {ex.Message}");
			}
			return StatusCode(500);
		}
    }
}
