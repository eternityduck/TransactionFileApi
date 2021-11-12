using System.IO;
using Domain;
using MediatR;

namespace Application.TransactionsCqrs.Queries.GetByType
{
    public class GetByTypeQuery : IRequest<MemoryStream>
    {
        public TransactionType TransactionType { get; set; }

        public GetByTypeQuery(TransactionType type) =>
            TransactionType = type;
    }
}