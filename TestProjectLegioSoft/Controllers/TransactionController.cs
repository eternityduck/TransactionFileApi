using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace TestProjectLegioSoft.Controllers
{
    [Authorize]
    [Route("controller")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService) =>
            _transactionService = transactionService;
        
        
        [HttpGet]
        public async Task<IEnumerable<Transaction>> Index()
        {
            var transactions = await _transactionService.GetAllAsync();
            return transactions;
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            await _transactionService.DeleteByIdAsync(id);
            return Ok();
        }
        [HttpPut("id")]
        public async Task<IActionResult> Edit(int id, string status)
        {
            bool isParsed = Enum.TryParse(status, out Status statusParsed);
            if (!isParsed) return BadRequest("Incorrect status");
            
            await _transactionService.UpdateStatus(id, statusParsed);
            return Ok("Successfully updated the status");
        }
        [HttpPost("Import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
           await _transactionService.ImportFileAsync(file);
           return Ok();
        }

        [HttpGet("Export/FilterByType")]
        public async Task<IActionResult> ExportByType(string type)
        {
            bool isParsed = Enum.TryParse(type, out TransactionType typeParsed);
            if (!isParsed) return BadRequest("Incorrect type name");
            
            var file = await _transactionService.GetByTypeAsync(typeParsed);
            
            return File(file, "text/csv", "sample.csv");
        }

        [HttpGet("Export/FilterByStatus")]
        public async Task<IActionResult> ExportByStatus(string status)
        {
            bool isParsed = Enum.TryParse(status, out Status typeParsed);
            if (!isParsed) return BadRequest("Incorrect type name");
            
            var file = await _transactionService.GetByStatusAsync(typeParsed);
            
            return File(file, "text/csv", "sample.csv");
        }

        [HttpGet("Export/FilterByClientName")]
        public async Task<IActionResult> ExportByClientName(string name)
        {
            var file = await _transactionService.GetByClientNameAsync(name);
            
            return File(file, "text/csv", "sample.csv");
        }
    }
}