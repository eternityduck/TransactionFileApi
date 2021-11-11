using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using DAL;
using Domain;
using MediatR;

namespace BLL.TransactionsCqrs.Commands.ImportFile
{
    public class ImportFileHandler: IRequestHandler<ImportFileCommand>
    {
        private readonly ApiContext _context;

        public ImportFileHandler(ApiContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(ImportFileCommand request, CancellationToken cancellationToken)
        {
            if (!request.File.FileName.EndsWith(".csv"))
            {
                throw new ArgumentException("The file isn`t in the right format. Upload the .csv file");
            }
            
            using (var stream = new StreamReader(request.File.OpenReadStream()))
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
            
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}