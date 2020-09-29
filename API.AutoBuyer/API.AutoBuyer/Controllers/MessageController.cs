using System;
using System.Linq;
using AutoBuyer.API.Models;
using AutoBuyer.API.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoBuyer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("message")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageProvider _provider;

        public MessageController(IMessageProvider provider)
        {
            _provider = provider;
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] Message messageBody)
        {
            try
            {
                var email = User.Claims.First(x => x.Type == "Email");

                _provider.Send(messageBody.Subject, messageBody.Body, email.Value);

                return Ok("Sent");
            }
            catch (Exception ex)
            {
                return Problem("Something is broke, yo"); //TODO: Probably shouldn't do this
            }
        }
    }
}
