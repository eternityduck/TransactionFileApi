using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.TransactionsCqrs.Queries.GetAll
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, IEnumerable<Transaction>>
    {
        private readonly ApiContext _context;

        public GetAllHandler(ApiContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Transaction>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            return await _context.Transactions.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
        }
    }
}