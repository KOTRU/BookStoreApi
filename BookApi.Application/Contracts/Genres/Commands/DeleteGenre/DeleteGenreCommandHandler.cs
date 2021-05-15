using System.Threading;
using System.Threading.Tasks;
using BookApi.Application.Common.Database;
using MediatR;

namespace BookApi.Application.Contracts.Genres.Commands.DeleteGenre
{
    internal class DeleteGenreCommandHandler:IRequestHandler<DeleteGenreCommand, bool>
    {
        private readonly IDbContext _dbContext;

        public DeleteGenreCommandHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.GetDb();
            var genre = await _dbContext.GetGenre(request.Id);
            if (genre is null) return false;
            await _dbContext.DeleteGenre(request.Id);
            return true;
        }
    }
}