using System;
using AutoBuyer.API.Models;
using AutoBuyer.API.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoBuyer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthProvider _authService;

        public AuthController(IAuthProvider authProvider)
        {
            _authService = authProvider;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult Authenticate([FromBody]AuthRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("You must send in a user name and password");
                }

                var response = _authService.Authenticate(request.Username, request.Password);

                if (response.Authenticated)
                {
                    return Ok(response);
                }
                else
                {
                    return Unauthorized("Invalid user or password");
                }
            }
            catch (Exception ex)
            {
                return Problem("Something is broke, yo"); //TODO: Probably shouldn't do this
            }
        }
    }
}