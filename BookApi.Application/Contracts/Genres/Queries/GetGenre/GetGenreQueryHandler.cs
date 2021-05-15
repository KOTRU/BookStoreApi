using System.Threading;
using System.Threading.Tasks;
using BookApi.Application.Common.Database;
using BookApi.Domain.Entities;
using MediatR;

namespace BookApi.Application.Contracts.Genres.Queries.GetGenre
{
    public class GetGenreQueryHandler : IRequestHandler<GetGenreQuery, Genre>
    {
        private readonly IDbContext _dbContext;

        public GetGenreQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Genre> Handle(GetGenreQuery request, CancellationToken cancellationToken)
        {
            await _dbContext.GetDb();
            return await _dbContext.GetGenre(request.Id);
        }
    }
}