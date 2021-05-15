using System.Threading;
using System.Threading.Tasks;
using BookApi.Application.Common.Database;
using BookApi.Domain.Entities;
using MediatR;

namespace BookApi.Application.Contracts.Authors.Queries.GetAuthor
{
    internal class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, Author>
    {
        private readonly IDbContext _dbContext;

        public GetAuthorQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Author> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            await _dbContext.GetDb();
            return await _dbContext.GetAuthor(request.Id);
        }
    }
}