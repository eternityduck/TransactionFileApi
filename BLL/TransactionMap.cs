using CsvHelper.Configuration;
using DAL.Models;

namespace DAL.CsvParser
{
    public sealed class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Map(m => m.Id).Index(0).Name("TransactionId");
            Map(m => m.Client).Index(1).Name("ClientName");
            Map(m => m.Status).Index(2).Name("Status");
            Map(m => m.TransactionType).Index(3).Name("Type");
            Map(m => m.Amount).Index(4).Name("Amount");
        }
    }
}