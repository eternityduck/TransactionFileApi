using System.IO;
using MediatR;

namespace BLL.TransactionsCqrs.Queries.GetByClientName
{
    public class GetByClientNameQuery : IRequest<MemoryStream>
    {
        public string ClientName { get; set; }

        public GetByClientNameQuery(string name) =>
            ClientName = name;
    }
}