using System.Collections.Generic;
using System.IO;
using Domain;
using MediatR;

namespace BLL.TransactionsCqrs.Queries.GetAll
{
    public class GetAllQuery : IRequest<MemoryStream>
    {
        
    }
}