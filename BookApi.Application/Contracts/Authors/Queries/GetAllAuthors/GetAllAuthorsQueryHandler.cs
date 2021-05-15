using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookApi.Application.Common.Database;
using BookApi.Domain.Entities;
using MediatR;

namespace BookApi.Application.Contracts.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler:IRequestHandler<GetAllAuthorsQuery, IList<Author>>
    {
        private readonly IDbContext _dbContext;

        public GetAllAuthorsQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IList<Author>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            await _dbContext.GetDb();
            return await _dbContext.GetAuthors();
        }
    }
}