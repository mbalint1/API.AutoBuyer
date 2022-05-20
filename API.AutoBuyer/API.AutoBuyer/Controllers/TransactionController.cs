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
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionProvider _transactionService;

        public TransactionController(ITransactionProvider service)
        {
            _transactionService = service;
        }

        /// <summary>
        /// Saves a transaction log from Desktop.AutoBuyer
        /// </summary>
        /// <response code="200">Saved transaction log returned</response>
        /// <response code="400">Missing Required Data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">API Error</response>
        [ProducesResponseType(typeof(TransactionLog), 200)]
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
                return StatusCode(500, "Error in API. Please try again later.");
            }
        }

        /// <summary>
        /// Gets transaction logs by user and date filters
        /// </summary>
        /// <response code="200">Transaction logs returned</response>
        /// <response code="400">Missing Required Data</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">API Error</response>
        [ProducesResponseType(typeof(List<TransactionLog>), 200)]
        [HttpGet]
        public IActionResult GetTransactions([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var user = User.Claims.FirstOrDefault(x => x.Type == "User");

                if (user == null || string.IsNullOrEmpty(user.Value))
                {
                    return Unauthorized("Invalid Token");
                }

                if (startDate == null || endDate == null)
                {
                    return BadRequest("start and end dates are required filtering parameters");
                }

                return Ok(_transactionService.GetTransactionLogs(user.Value, startDate, endDate));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error in API. Please try again later.");
            }
        }
    }
}