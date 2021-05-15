using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using BookApi.Application.Common.Database;
using BookApi.Application.Contracts.Authors.Commands.CreateNewAuthor;
using BookApi.Application.Contracts.Authors.Commands.DeleteAuthor;
using BookApi.Application.Contracts.Authors.Queries.GetAllAuthors;
using BookApi.Application.Contracts.Authors.Queries.GetAuthor;
using BookApi.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BookApi.Application.Tests
{
    public class AuthorsTests
    {
        private readonly IDbContext _dbContext;
        private readonly Faker<Author> _authorFaker;

        public AuthorsTests()
        {
            _dbContext = Substitute.For<IDbContext>();
            _authorFaker = new Faker<Author>()
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                .RuleFor(x => x.LastName, f => f.Person.LastName)
                .RuleFor(x => x.BirthYear, f => f.Person.DateOfBirth)
                .RuleFor(x => x.DeathYear, f => f.Date.Future());
        }

        [Fact]
        public async Task GetAllAuthorsTest()
        {
            var authorsReturns = _authorFaker.GenerateForever().Take(10).ToList();
            _dbContext.GetAuthors().Returns(authorsReturns);
            var getAllAuthorsQueryHandler = new GetAllAuthorsQueryHandler(_dbContext);
            var getAllAuthorsQuery = new GetAllAuthorsQuery();
            var authors = await getAllAuthorsQueryHandler.Handle(getAllAuthorsQuery, CancellationToken.None);
            authors.Should().Equal(authorsReturns);
        }

        [Fact]
        public async Task GetAuthorTest()
        {
            var authorToGet = _authorFaker.Generate();
            _dbContext.GetAuthor(authorToGet.Id).Returns(authorToGet);

            var getAuthorQueryHandler = new GetAuthorQueryHandler(_dbContext);
            var getAuthorQuery = new GetAuthorQuery
            {
                Id = authorToGet.Id
            };
            var author = await getAuthorQueryHandler.Handle(getAuthorQuery, CancellationToken.None);
            author.Should().Be(authorToGet);
        }

        [Fact]
        public async Task AddNewAuthorTestNormal()
        {
            var authorToAdd = _authorFaker.Generate();
            _dbContext.GetAuthor(authorToAdd.Id).Returns(_ => (Author) null);

            var createNewAuthorCommandHandler = new CreateNewAuthorCommandHandler(_dbContext);
            var createNewAuthorCommand = new CreateNewAuthorCommand
            {
                Author = authorToAdd
            };
            var result = await createNewAuthorCommandHandler.Handle(createNewAuthorCommand, CancellationToken.None);
            result.Should().Be(true);
        }

        [Fact]
        public async Task AddNewAuthorTestFail()
        {
            var authorToAdd = _authorFaker.Generate();
            _dbContext.GetAuthor(authorToAdd.Id).Returns(_ => authorToAdd);

            var createNewAuthorCommandHandler = new CreateNewAuthorCommandHandler(_dbContext);
            var createNewAuthorCommand = new CreateNewAuthorCommand
            {
                Author = authorToAdd
            };
            var result = await createNewAuthorCommandHandler.Handle(createNewAuthorCommand, CancellationToken.None);
            result.Should().Be(false);
        }

        [Fact]
        public async Task DeleteAuthorTestNormal()
        {
            var authorToDelete = _authorFaker.Generate();
            _dbContext.GetAuthor(authorToDelete.Id).Returns(authorToDelete);

            var deleteAuthorCommandHandler = new DeleteAuthorCommandHandler(_dbContext);
            var deleteAuthorCommand = new DeleteAuthorCommand
            {
                Id = authorToDelete.Id
            };
            var deleted = await deleteAuthorCommandHandler.Handle(deleteAuthorCommand, CancellationToken.None);
            deleted.Should().Be(true);
        }

        [Fact]
        public async Task DeleteGenreTestFail()
        {
            var authorToDelete = _authorFaker.Generate();
            _dbContext.GetAuthor(authorToDelete.Id).Returns(_ => (Author) null);

            var deleteAuthorCommandHandler = new DeleteAuthorCommandHandler(_dbContext);
            var deleteAuthorCommand = new DeleteAuthorCommand
            {
                Id = authorToDelete.Id
            };
            var deleted = await deleteAuthorCommandHandler.Handle(deleteAuthorCommand, CancellationToken.None);
            deleted.Should().Be(false);
        }
    }
}