using System;
using System.Linq;
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

        /// <summary>
        /// Starts an autobuyer session for a desktop user for a given player
        /// </summary>
        /// <response code="200">Session info returned</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">API Error</response>
        [ProducesResponseType(typeof(SessionDTO), 200)]
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

                var sessionId = _provider.StartSession(sessionInfo.PlayerVersionId, userId.Value, sessionInfo.SearchNum);

                sessionInfo.SessionId = sessionId;

                return Ok(sessionInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error in API. Please try again later.");
            }
        }

        /// <summary>
        /// Ends an autobuyer session for a desktop user for a given player
        /// </summary>
        /// <response code="200">Session Ended</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">API Error</response>
        [ProducesResponseType(200)]
        [HttpPut]
        public IActionResult EndSession([FromBody] SessionDTO sessionInfo)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId");

                if (userId == null)
                {
                    return Unauthorized("Invalid Token");
                }

                //TODO: We should probably use the claim userId to validate it's their session
                _provider.EndSession(sessionInfo.SessionId, sessionInfo.PlayerVersionId, sessionInfo.Captcha, sessionInfo.PurchasedNum);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error in API. Please try again later.");
            }
        }
    }
}