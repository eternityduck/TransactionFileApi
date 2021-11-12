using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.TransactionsCqrs.Commands.DeleteById;
using Application.TransactionsCqrs.Commands.ImportFile;
using Application.TransactionsCqrs.Commands.UpdateStatus;
using Application.TransactionsCqrs.Queries.GetAll;
using Application.TransactionsCqrs.Queries.GetByClientName;
using Application.TransactionsCqrs.Queries.GetByStatus;
using Application.TransactionsCqrs.Queries.GetByType;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class TransactionController : BaseController
    {
        /// <summary>
        /// Export all of transactions
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /Transaction
        /// </remarks>
        /// <returns>Returns file that contains all transactions from DB</returns>
        /// <response code ="200">Success</response>
        /// <response code ="401">If the user is unauthorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Transaction>>> ExportAll( CancellationToken token)
        {
            var query = new GetAllQuery();
            var file = await Mediator.Send(query, token);
            return File(file, "text/csv", "sample.csv");
        }
        /// <summary>
        /// Deletes transaction by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE/transaction/1
        /// </remarks>
        /// <param name="id">Transaction id (int)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code ="204">Success</response>
        /// <response code ="401">If the user is unauthorized</response>
        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteByIdCommand(id);
            await Mediator.Send(command);
            
            return NoContent();
        }
        /// <summary>
        /// Updates the status of transaction
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Transaction/PUT/1
        /// </remarks>
        /// <param name="id">Transaction id (int)</param>
        /// <param name="status">Updated status (string)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code ="204">Success</response>
        /// <response code ="401">If the user is unauthorized</response>
        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Edit(int id, string status)
        {
            bool isParsed = Enum.TryParse(status, out Status statusParsed);
            if (!isParsed) return BadRequest("Incorrect status");

            var command = new UpdateStatusCommand(statusParsed, id);
            await Mediator.Send(command);
            
            return NoContent();
        }
        /// <summary>
        /// Imports the file to the database or updates the status if the transaction already in the database
        /// </summary>
        /// POST/Transaction
        /// <param name="file">File to import (.csv file)</param>
        /// <returns>NoContent</returns>
        /// <response code ="204">Success</response>
        /// <response code ="401">If the user is unauthorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var command = new ImportFileCommand(file);
            await Mediator.Send(command);
            return NoContent();
        }
        /// <summary>
        /// Filters and exports the file by the type
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET/Refill 
        /// </remarks>
        /// <param name="type">The type which will be a filter (string)</param>
        /// <returns>.csv File</returns>
        /// <response code ="200">Success</response>
        /// <response code ="401">If the user is unauthorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ExportByType(string type)
        {
            bool isParsed = Enum.TryParse(type, out TransactionType typeParsed);
            if (!isParsed) return BadRequest("Incorrect type name");

            var query = new GetByTypeQuery(typeParsed);
            var file = await Mediator.Send(query);
            return File(file, "text/csv", "sample.csv");
        }
        /// <summary>
        /// Filters and exports the file by the status
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET/Completed
        /// </remarks>
        /// <param name="status">The status which will be a filter (string)</param>
        /// <returns>.csv File</returns>
        /// <response code ="200">Success</response>
        /// <response code ="401">If the user is unauthorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ExportByStatus(string status)
        {
            bool isParsed = Enum.TryParse(status, out Status typeParsed);
            if (!isParsed) return BadRequest("Incorrect type name");
            var query = new GetByStatusQuery(typeParsed);
            
            var file = await Mediator.Send(query);
            return File(file, "text/csv", "sample.csv");
        }
        /// <summary>
        /// Filters and exports the file by the client name
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET/John
        /// </remarks>
        /// <param name="name">The client`s name (string)</param>
        /// <returns>.csv File</returns>
        /// <response code ="200">Success</response>
        /// <response code ="401">If the user is unauthorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ExportByClientName(string name)
        {
            var query = new GetByClientNameQuery(name);
            
            var file = await Mediator.Send(query);
            return File(file, "text/csv", "sample.csv");
        }
    }
}