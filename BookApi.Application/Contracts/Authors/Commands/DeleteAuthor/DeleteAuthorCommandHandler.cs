using System.Threading;
using System.Threading.Tasks;
using BookApi.Application.Common.Database;
using MediatR;

namespace BookApi.Application.Contracts.Authors.Commands.DeleteAuthor
{
    internal class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
    {
        private readonly IDbContext _dbContext;

        public DeleteAuthorCommandHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.GetDb();
            var author = await _dbContext.GetAuthor(request.Id);
            if (author is null) return false;
            await _dbContext.DeleteAuthor(request.Id);
            return true;
        }
    }
}