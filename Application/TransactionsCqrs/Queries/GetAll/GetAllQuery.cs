using System.Collections.Generic;
using Domain;
using MediatR;

namespace BLL.TransactionsCqrs.Queries.GetAll
{
    public class GetAllQuery : IRequest<IEnumerable<Transaction>>
    {
        
    }
}