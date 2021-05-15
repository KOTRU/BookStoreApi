using System;
using MediatR;

namespace BookApi.Application.Contracts.Genres.Commands.DeleteGenre
{
    public class DeleteGenreCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}