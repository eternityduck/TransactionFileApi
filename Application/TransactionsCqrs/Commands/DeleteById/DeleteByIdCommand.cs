using MediatR;

namespace BLL.TransactionsCqrs.Commands.DeleteById
{
    public class DeleteByIdCommand : IRequest
    {
        public int Id { get; set; }

        public DeleteByIdCommand(int id) =>
            Id = id;
    }
}