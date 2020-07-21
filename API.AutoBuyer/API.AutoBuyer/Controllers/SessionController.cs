using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBuyer.API.Models;
using AutoBuyer.API.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoBuyer.API.Controllers
{
    [Authorize]
    [Route("api/sessions")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionProvider _provider;

        public SessionController(ISessionProvider provider)
        {
            _provider = provider;
        }

        [HttpPost]
        public IActionResult CreateSession([FromBody] SessionDTO sessionInfo)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId");

                if (userId == null)
                {
                    return Unauthorized("Invalid Token");
                }

                var sessionId = _provider.StartSession(sessionInfo.PlayerVersionId, userId.Value);

                sessionInfo.SessionId = sessionId;

                return Ok(sessionInfo);
            }
            catch (Exception ex)
            {
                return Problem("Something is broke, yo");
            }
        }

        [HttpPut]
        public IActionResult EndSession([FromBody] SessionDTO sessionInfo)
        {
            try
            {
                _provider.EndSession(sessionInfo.SessionId, sessionInfo.PlayerVersionId);

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem("Something is broke, yo");
            }
        }
    }
}