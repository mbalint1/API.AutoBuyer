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
        public IActionResult CreateSession([FromBody] SessionInfo sessionInfo)
        {
            try
            {
                //var sessionId = _provider.StartSession();

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem("Something is broke, yo");
            }
        }

        [HttpPut]
        public IActionResult EndSession()
        {
            try
            {
                //_provider.EndSession();

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem("Something is broke, yo");
            }
        }
    }
}