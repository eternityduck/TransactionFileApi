using System;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using MediatR;

namespace BLL.TransactionsCqrs.Commands.UpdateStatus
{
    public class UpdateStatusHandler : IRequestHandler<UpdateStatusCommand>
    {
        private readonly ApiContext _context;

        public UpdateStatusHandler(ApiContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var model = await _context.Transactions.FindAsync(request.Id);
            if (model is null)
            {
                throw new ArgumentOutOfRangeException(nameof(request.Id), "Incorrect Id");
            }
            model.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}