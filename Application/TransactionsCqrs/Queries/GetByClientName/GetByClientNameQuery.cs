using System.IO;
using MediatR;

namespace Application.TransactionsCqrs.Queries.GetByClientName
{
    public class GetByClientNameQuery : IRequest<MemoryStream>
    {
        public string ClientName { get; set; }

        public GetByClientNameQuery(string name) =>
            ClientName = name;
    }
}