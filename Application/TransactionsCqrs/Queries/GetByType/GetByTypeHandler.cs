using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.TransactionsCqrs.Queries.GetByType
{
    public class GetByTypeHandler : QueryBase, IRequestHandler<GetByTypeQuery, MemoryStream>
    {
        private readonly ApiContext _context;

        public GetByTypeHandler(ApiContext context)
        {
            _context = context;
        }
        public async Task<MemoryStream> Handle(GetByTypeQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Transactions
                .AsNoTracking()
                .Where(x => x.TransactionType == request.TransactionType)
                .ToListAsync(cancellationToken: cancellationToken);
            var arr = await CreateCsvFile(result);
            return ConvertToMemoryStream(arr);
        }
    }
}