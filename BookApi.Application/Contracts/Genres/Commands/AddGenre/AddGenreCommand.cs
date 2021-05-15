using BookApi.Domain.Entities;
using MediatR;

namespace BookApi.Application.Contracts.Genres.Commands.AddGenre
{
    public class AddGenreCommand:IRequest<bool>
    {
        public Genre Genre { get; set; }
    }
}