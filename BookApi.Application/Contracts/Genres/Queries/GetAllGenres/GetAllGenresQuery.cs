using System.Collections.Generic;
using BookApi.Domain.Entities;
using MediatR;

namespace BookApi.Application.Contracts.Genres.Queries.GetAllGenres
{
    public class GetAllGenresQuery:IRequest<IList<Genre>>
    {
    }
}