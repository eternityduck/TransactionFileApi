using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.TransactionsCqrs.Queries.GetByStatus
{
    public class GetByStatusHandler : QueryBase, IRequestHandler<GetByStatusQuery, MemoryStream>
    {
        private readonly ApiContext _context;

        public GetByStatusHandler(ApiContext context)
        {
            _context = context;
        }


        public async Task<MemoryStream> Handle(GetByStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Transactions
                .AsNoTracking()
                .Where(x => x.Status == request.Status)
                .ToListAsync(cancellationToken);
            var arr = await CreateCsvFile(result);
            return ConvertToMemoryStream(arr);
            
        }
    }
}