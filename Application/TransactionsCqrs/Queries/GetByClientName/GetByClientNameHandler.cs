using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.TransactionsCqrs.Queries.GetByClientName
{
    public class GetByClientNameHandler : QueryBase, IRequestHandler<GetByClientNameQuery, MemoryStream>
    {
        private readonly ApiContext _context;

        public GetByClientNameHandler(ApiContext context)
        {
            _context = context;
        }
        
        public async Task<MemoryStream> Handle(GetByClientNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Transactions
                .AsNoTracking()
                .Where(x => x.Client.ToLower().Contains(request.ClientName.ToLower()))
                .ToListAsync(cancellationToken: cancellationToken);
            var arr = await CreateCsvFile(result);
            return ConvertToMemoryStream(arr);
            
        }
    }
}