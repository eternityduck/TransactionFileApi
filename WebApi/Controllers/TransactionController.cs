using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.TransactionsCqrs.Commands.DeleteById;
using BLL.TransactionsCqrs.Commands.ImportFile;
using BLL.TransactionsCqrs.Commands.UpdateStatus;
using BLL.TransactionsCqrs.Queries.GetAll;
using BLL.TransactionsCqrs.Queries.GetByClientName;
using BLL.TransactionsCqrs.Queries.GetByStatus;
using BLL.TransactionsCqrs.Queries.GetByType;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace TestProjectLegioSoft.Controllers
{
    [Authorize]
    public class TransactionController : BaseController
    {
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAll()
        {
            var query = new GetAllQuery();
            var transactions = await Mediator.Send(query);
            return Ok(transactions);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteByIdCommand(id);
            await Mediator.Send(command);
            
            return NoContent();
        }
        [HttpPut("id")]
        public async Task<IActionResult> Edit(int id, string status)
        {
            bool isParsed = Enum.TryParse(status, out Status statusParsed);
            if (!isParsed) return BadRequest("Incorrect status");

            var command = new UpdateStatusCommand(statusParsed, id);
            await Mediator.Send(command);
            
            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var command = new ImportFileCommand(file);
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> ExportByType(string type)
        {
            bool isParsed = Enum.TryParse(type, out TransactionType typeParsed);
            if (!isParsed) return BadRequest("Incorrect type name");

            var query = new GetByTypeQuery(typeParsed);
            var file = await Mediator.Send(query);
            return File(file, "text/csv", "sample.csv");
        }

        [HttpGet]
        public async Task<IActionResult> ExportByStatus(string status)
        {
            bool isParsed = Enum.TryParse(status, out Status typeParsed);
            if (!isParsed) return BadRequest("Incorrect type name");
            var query = new GetByStatusQuery(typeParsed);
            
            var file = await Mediator.Send(query);
            return File(file, "text/csv", "sample.csv");
        }

        [HttpGet]
        public async Task<IActionResult> ExportByClientName(string name)
        {
            var query = new GetByClientNameQuery(name);
            
            var file = await Mediator.Send(query);
            return File(file, "text/csv", "sample.csv");
        }
    }
}