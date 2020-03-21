using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBuyer.API.Core.DTO;
using AutoBuyer.API.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoBuyer.API.Controllers
{
    [Authorize]
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionProvider _transactionService;

        public TransactionController(ITransactionProvider service)
        {
            _transactionService = service;
        }

        [HttpPost]
        public IActionResult InsertTransaction([FromBody]TransactionLog log)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == "User");

                if (user != null && !string.IsNullOrEmpty(user.Value))
                {
                    log.UserName = user.Value;
                }

                _transactionService.InsertTransactionLog(log);

                if (log.TransactionId.Length > 0)
                {
                    return Ok(log);
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