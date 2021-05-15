using System.Threading;
using System.Threading.Tasks;
using BookApi.Application.Common.Database;
using MediatR;

namespace BookApi.Application.Contracts.Genres.Commands.AddGenre
{
    internal class AddGenreCommandHandler : IRequestHandler<AddGenreCommand, bool>
    {
        private readonly IDbContext _dbContext;

        public AddGenreCommandHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(AddGenreCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.GetDb();
            var genre = await _dbContext.GetGenre(request.Genre.Id);
            if (genre is not null) return false;
            await _dbContext.AddGenre(request.Genre);
            return true;
        }
    }
}