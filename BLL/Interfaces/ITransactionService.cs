using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Http;


namespace BLL.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task DeleteByIdAsync(int id);
        Task UpdateStatus(int id, Status status);
        Task<MemoryStream> GetByStatusAsync(Status status);
        Task<MemoryStream> GetByTypeAsync(TransactionType type);
        Task<MemoryStream> GetByClientNameAsync(string name);
        Task ImportFileAsync(IFormFile file);
    }
}