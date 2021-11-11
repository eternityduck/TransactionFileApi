using System;
using System.IO;
using CsvHelper;
using DAL.Models;
using System.Globalization;

namespace DAL
{
    public class DbInitializer
    {
        public static void Initialize(ApiContext context)
        {
             context.Database.EnsureCreated();
             Import(context);
        }

        private static void Import(ApiContext context)
        {
            using(var stream = new StreamReader(@"D:\TestProjectLegioSoft\DAL\Domain\SeedData\data.csv"))
            using(var csv = new CsvReader(stream, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    int id = Int32.Parse(csv.GetField("TransactionId"));
                    Status status = (Status)Enum.Parse(typeof(Status), csv.GetField("Status"));
                    TransactionType type = (TransactionType)Enum.Parse(typeof(TransactionType), csv.GetField("Type"));
                    string clientName = csv.GetField("ClientName");
                    var amount = Decimal.Parse(csv.GetField("Amount").Remove(0, 1));

                    var record = context.Transactions.Find(id);
                    if (record == null)
                    {
                        context.Transactions.Add(new Transaction()
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

            context.SaveChanges();
        }
    }
}