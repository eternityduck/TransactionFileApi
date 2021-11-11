using System;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using MediatR;

namespace BLL.TransactionsCqrs.Commands.DeleteById
{
    public class DeleteByIdHandler : IRequestHandler<DeleteByIdCommand>
    {
        private readonly ApiContext _context;

        public DeleteByIdHandler(ApiContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
        {
            var model = await _context.Transactions.FindAsync(request.Id);
            if (model is null)
            {
                throw new ArgumentOutOfRangeException(nameof(request.Id), "Incorrect Id");
            }
            _context.Transactions.Remove(model);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}