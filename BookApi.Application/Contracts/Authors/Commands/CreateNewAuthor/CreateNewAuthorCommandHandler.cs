using System.Threading;
using System.Threading.Tasks;
using BookApi.Application.Common.Database;
using MediatR;

namespace BookApi.Application.Contracts.Authors.Commands.CreateNewAuthor
{
    internal class CreateNewAuthorCommandHandler:IRequestHandler<CreateNewAuthorCommand, bool>
    {
        private readonly IDbContext _dbContext;

        public CreateNewAuthorCommandHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Handle(CreateNewAuthorCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.GetDb();
            var author = await _dbContext.GetAuthor(request.Author.Id);
            if (author is not null) return false;
            await _dbContext.AddAuthor(request.Author);
            return true;
        }
    }
}