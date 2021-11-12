using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TransactionsCqrs.Queries.GetAll
{
    public class GetAllHandler : QueryBase, IRequestHandler<GetAllQuery, MemoryStream>
    {
        private readonly ApiContext _context;

        public GetAllHandler(ApiContext context)
        {
            _context = context;
        }
        
        public async Task<MemoryStream> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Transactions
                .AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);
            var arr = await CreateCsvFile(result);
            return ConvertToMemoryStream(arr);
        }
    }
}