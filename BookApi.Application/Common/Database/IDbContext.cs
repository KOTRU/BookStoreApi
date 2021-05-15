using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookApi.Domain.Entities;

namespace BookApi.Application.Common.Database
{
    public interface IDbContext
    {
        // public Task AddBook(Book book, Author author, Genre genre);
        // public Task<IEnumerable<Post>> GetPosts(Genre genre, int limit);
        // Task<IEnumerable<Book>> GetBooks(IEnumerable<Post> posts);
        Task<IDbContext> GetDb();

        #region Genre

        Task AddGenre(Genre genre);
        Task DeleteGenre(Guid requestGenreId);

        #region Get

        Task<IList<Genre>> GetGenres();
        Task<Genre> GetGenre(Guid requestGenreId);

        #endregion

        #endregion

        #region Author

        Task AddAuthor(Author requestAuthor);
        Task DeleteAuthor(Guid requestId);

        #region Get

        Task<Author> GetAuthor(Guid authorId);
        Task<IList<Author>> GetAuthors();

        #endregion

        #endregion

    }
}