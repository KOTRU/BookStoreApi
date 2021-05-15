using System;
using BookApi.Domain.Entities;
using MediatR;

namespace BookApi.Application.Contracts.Genres.Queries.GetGenre
{
    public class GetGenreQuery:IRequest<Genre>
    {
        public Guid Id { get; set; }
    }
}