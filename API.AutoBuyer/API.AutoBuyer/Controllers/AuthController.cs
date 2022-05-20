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

        /// <summary>
        /// Used to authenticate and get an access token
        /// </summary>
        /// <response code="200">Authenticated, token returned</response>
        /// <response code="400">Missing Required Data</response>
        /// <response code="401">Unauthorized</response>
        [ProducesResponseType(typeof(AuthResponse), 200)]
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
                return Unauthorized("Something is broke, yo");
            }
        }
    }
}