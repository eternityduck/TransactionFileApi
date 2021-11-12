using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Application.Common.Mappings;
using CsvHelper;
using Domain;

namespace Application.TransactionsCqrs.Queries
{
    public class QueryBase
    {
        protected async Task<byte[]> CreateCsvFile(IEnumerable<Transaction> transactions)
        {
            await using var memStream = new MemoryStream();
            await using var streamWriter = new StreamWriter(memStream);
            await using var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<TransactionMap>();
            csv.WriteHeader<Transaction>();
            await csv.NextRecordAsync();

            foreach (var transaction in transactions)
            {
                csv.WriteRecord(transaction);
                await csv.NextRecordAsync();
            }

            await streamWriter.FlushAsync();
            await csv.FlushAsync();
            return memStream.ToArray();
        }

        protected MemoryStream ConvertToMemoryStream(byte[] arr)
        {
            return new MemoryStream(arr);
        }
    }
}