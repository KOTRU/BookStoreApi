using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookApi.Application.Common.Database;
using BookApi.Domain.Entities;
using MediatR;

namespace BookApi.Application.Contracts.Genres.Queries.GetAllGenres
{
    internal class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresQuery, IList<Genre>>
    {
        private readonly IDbContext _dbContext;

        public GetAllGenresQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Genre>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
        {
            await _dbContext.GetDb();
            return await _dbContext.GetGenres();
        }
    }
}