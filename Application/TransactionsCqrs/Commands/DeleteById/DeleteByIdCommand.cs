using MediatR;

namespace Application.TransactionsCqrs.Commands.DeleteById
{
    public class DeleteByIdCommand : IRequest
    {
        public int Id { get; set; }

        public DeleteByIdCommand(int id) =>
            Id = id;
    }
}