using System;
using System.Linq;
using AutoBuyer.API.Core.Utilities;
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

        ////TODO: This doesn't work when hosted in Azure. They block SMTP traffic.
        //[HttpPost]
        //public IActionResult SendMessage([FromBody] Message messageBody)
        //{
        //    try
        //    {
        //        var email = User.Claims.First(x => x.Type == "Email");

        //        _provider.Send(messageBody.Subject, messageBody.Body, email.Value);

        //        return Ok("Sent");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Problem("Something is broke, yo"); //TODO: Probably shouldn't do this
        //    }
        //}

        /// <summary>
        /// Gets message related information needed for Desktop.Autobuyer to send emails
        /// </summary>
        /// <response code="200">Message data returned</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">API Error</response>
        [ProducesResponseType(typeof(string), 200)]
        [HttpGet]
        [Route("data")]
        public IActionResult GetMessageData()
        {
            try
            {
                var email = User.Claims.FirstOrDefault(x => x.Type == "Email");

                if (email == null)
                {
                    return Unauthorized("Invalid token");
                }

                return Ok($"{ConnectionUtility.GetEmailPassword()} {ConnectionUtility.GetFromEmail()} {email}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error in API. Please try again later.");
            }
        }
    }
}
