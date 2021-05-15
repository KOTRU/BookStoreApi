using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using BookApi.Application.Common.Database;
using BookApi.Application.Contracts.Genres.Commands.AddGenre;
using BookApi.Application.Contracts.Genres.Commands.DeleteGenre;
using BookApi.Application.Contracts.Genres.Queries.GetAllGenres;
using BookApi.Application.Contracts.Genres.Queries.GetGenre;
using BookApi.Domain.Entities;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BookApi.Application.Tests
{
    public class GenresTests
    {
        private readonly IDbContext _dbContext;
        private readonly Faker<Genre> _genreFaker;

        public GenresTests()
        {
            _dbContext = NSubstitute.Substitute.For<IDbContext>();
            _genreFaker = new Faker<Genre>()
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.Name, f => f.Commerce.Product());
        }

        [Fact]
        public async Task GetAllGenresTest()
        {
            var genresReturns = _genreFaker.GenerateForever().Take(10).ToList();
            _dbContext.GetGenres().Returns(genresReturns);
            var getAllGenresQueryHandler = new GetAllGenresQueryHandler(_dbContext);
            var getAllGenresQuery = new GetAllGenresQuery();
            var genres = await getAllGenresQueryHandler.Handle(getAllGenresQuery, CancellationToken.None);
            genres.Should().Equal(genresReturns);
        }

        [Fact]
        public async Task GetGenreTest()
        {
            var genreToGet = _genreFaker.Generate();
            _dbContext.GetGenre(genreToGet.Id).Returns(genreToGet);

            var getGenreQueryHandler = new GetGenreQueryHandler(_dbContext);
            var getGenreQuery = new GetGenreQuery
            {
                Id = genreToGet.Id
            };
            var deleted = await getGenreQueryHandler.Handle(getGenreQuery, CancellationToken.None);
            deleted.Should().Be(genreToGet);
        }

        [Fact]
        public async Task AddNewGenreTestNormal()
        {
            var genreToAdd = _genreFaker.Generate();
            _dbContext.GetGenre(genreToAdd.Id).Returns(info => (Genre) null);

            var addGenreCommandHandler = new AddGenreCommandHandler(_dbContext);
            var addGenreCommand = new AddGenreCommand
            {
                Genre = genreToAdd
            };
            var result = await addGenreCommandHandler.Handle(addGenreCommand, CancellationToken.None);
            result.Should().Be(true);
        }

        [Fact]
        public async Task AddNewGenreTestFail()
        {
            var genreToAdd = _genreFaker.Generate();
            _dbContext.GetGenre(genreToAdd.Id).Returns(info => genreToAdd);

            var addGenreCommandHandler = new AddGenreCommandHandler(_dbContext);
            var addGenreCommand = new AddGenreCommand
            {
                Genre = genreToAdd
            };
            var result = await addGenreCommandHandler.Handle(addGenreCommand, CancellationToken.None);
            result.Should().Be(false);
        }

        [Fact]
        public async Task DeleteGenreTestNormal()
        {
            var genreToDelete = _genreFaker.Generate();
            _dbContext.GetGenre(genreToDelete.Id).Returns(genreToDelete);

            var deleteGenreCommandHandler = new DeleteGenreCommandHandler(_dbContext);
            var deleteGenreCommand = new DeleteGenreCommand
            {
                Id = genreToDelete.Id
            };
            var deleted = await deleteGenreCommandHandler.Handle(deleteGenreCommand, CancellationToken.None);
            deleted.Should().Be(true);
        }

        [Fact]
        public async Task DeleteGenreTestFail()
        {
            var genreToDelete = _genreFaker.Generate();
            _dbContext.GetGenre(genreToDelete.Id).Returns(info => (Genre) null);

            var deleteGenreCommandHandler = new DeleteGenreCommandHandler(_dbContext);
            var deleteGenreCommand = new DeleteGenreCommand
            {
                Id = genreToDelete.Id
            };
            var deleted = await deleteGenreCommandHandler.Handle(deleteGenreCommand, CancellationToken.None);
            deleted.Should().Be(false);
        }
    }
}