using System.IO;
using Domain;
using MediatR;

namespace BLL.TransactionsCqrs.Queries.GetByType
{
    public class GetByTypeQuery : IRequest<MemoryStream>
    {
        public TransactionType TransactionType { get; set; }

        public GetByTypeQuery(TransactionType type) =>
            TransactionType = type;
    }
}