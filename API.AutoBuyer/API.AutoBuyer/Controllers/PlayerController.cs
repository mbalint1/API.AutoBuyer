using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBuyer.API.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public IActionResult GetPlayers()
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == "User");

                if (user == null)
                {
                    return Unauthorized("Bad token");
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
                return Problem("Something is broke, yo");
            }
        }
    }
}