using System;
using System.Collections.Generic;
using System.Linq;
using AutoBuyer.API.Core.DTO;
using AutoBuyer.API.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoBuyer.API.Controllers
{
    [Authorize]
    [Route("api/players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerProvider _playerService;

        public PlayerController(IPlayerProvider playerProvider)
        {
            _playerService = playerProvider;
        }

        /// <summary>
        /// Gets message related information needed for Desktop.Autobuyer to send emails
        /// </summary>
        /// <response code="200">Player list returned</response>
        /// <response code="400">Missing Required Data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">API Error</response>
        [ProducesResponseType(typeof(List<Player>), 200)]
        [HttpGet]
        public IActionResult GetPlayers()
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == "User");

                if (user == null)
                {
                    return Unauthorized("Invalid token");
                }

                var players = _playerService.GetPlayers();

                if (players.Count > 0)
                {
                    return Ok(players);
                }
                else
                {
                    return BadRequest("Something failed");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error in API. Please try again later.");
            }
        }
    }
}