using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using CsvHelper;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class TransactionService : ITransactionService
    {
        private static string _path = @$"..\DataStorage\data{Guid.NewGuid()}.csv";
        private readonly ApiContext _context;
        public TransactionService(ApiContext context) => _context = context;

        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var model = await GetByIdAsync(id);
            _context.Transactions.Remove(model);
        }

        public async Task UpdateStatus(int id, Status status)
        {
            var model = await GetByIdAsync(id);
            model.Status = status;
        }

        public async Task<MemoryStream> GetByStatusAsync(Status status)
        {
            var result = await _context.Transactions.Where(x => x.Status == status).ToListAsync();
            await CreateCsvFile(result);
            return await ExportMemoryStream();
        }
        
        public async Task<MemoryStream> GetByTypeAsync(TransactionType type)
        {
            var result = await _context.Transactions.Where(x => x.TransactionType == type).ToListAsync();
            await CreateCsvFile(result);
            return await ExportMemoryStream();
        }

        public async Task<MemoryStream> GetByClientNameAsync(string name)
        {
            var result = await _context.Transactions
                .Where(x => x.Client.ToLower().Contains(name.ToLower()))
                .ToListAsync();
            await CreateCsvFile(result);
            return await ExportMemoryStream();
        }


        public async Task ImportFileAsync(IFormFile file)
        {
            using (var stream = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(stream, CultureInfo.InvariantCulture))
            {
                await csv.ReadAsync();
                csv.ReadHeader();
                while (csv.Read())
                {
                    int id = Int32.Parse(csv.GetField("TransactionId"));
                    Status status = (Status)Enum.Parse(typeof(Status), csv.GetField("Status"));
                    TransactionType type = (TransactionType)Enum.Parse(typeof(TransactionType), csv.GetField("Type"));
                    string clientName = csv.GetField("ClientName");
                    var amount = Decimal.Parse(csv.GetField("Amount").Remove(0, 1));

                    var record = await _context.Transactions.FindAsync(id);
                    if (record == null)
                    {
                        _context.Transactions.Add(new Transaction()
                        {
                            Id = id,
                            Status = status,
                            TransactionType = type,
                            Client = clientName,
                            Amount = amount
                        });
                    }
                    else
                    {
                        record.Status = status;
                    }
                }
            }


            await _context.SaveChangesAsync();
        }

        private async Task CreateCsvFile(IEnumerable<Transaction> transactions)
        {
            var file = File.Create(_path);

            await using var writer = new StreamWriter(file);
            await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteHeader<Transaction>();
            await csv.NextRecordAsync();

            foreach (var transaction in transactions)
            {
                csv.WriteRecord(transaction);
                await csv.NextRecordAsync();
            }
        }

        private async Task<MemoryStream> ExportMemoryStream()
        {
            byte[] fileData;
            await using (FileStream fs = File.OpenRead(_path))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    fileData = binaryReader.ReadBytes((int)fs.Length);
                }
            }

            var dataStream = new MemoryStream(fileData);
            return dataStream;
        }
    }
}