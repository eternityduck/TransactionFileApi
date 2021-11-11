using Domain;
using MediatR;

namespace BLL.TransactionsCqrs.Commands.UpdateStatus
{
    public class UpdateStatusCommand : IRequest
    {
        public Status Status { get; set; }
        public int Id { get; set; }

        public UpdateStatusCommand(Status status, int id)
        {
            Status = status;
            Id = id;
        }
    }
}