using System.Collections.Generic;
using BookApi.Domain.Entities;
using MediatR;

namespace BookApi.Application.Contracts.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorsQuery:IRequest<IList<Author>>
    {
        
    }
}