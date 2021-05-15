using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookApi.Application.Common.Database;
using BookApi.Domain.Entities;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAuth = FirebaseAdmin.Auth.FirebaseAuth;

namespace BookApi.Infrastructure.Presistance
{
    public class FireBaseDatabase : IDbContext
    {
        private readonly ApiKey _apiKey;
        private readonly IMapper _mapper;
        private string _token;
        private FirebaseClient _firebaseClient;

        public FireBaseDatabase(ApiKey apiKey, IMapper mapper)
        {
            _apiKey = apiKey;
            _mapper = mapper;
        }

        public async Task<IDbContext> GetDb()
        {
            if (!string.IsNullOrEmpty(_token)) return this;
            Authorize();
            _token = await GetToken();
            GetDbClient();
            return this;
        }

        private void GetDbClient()
        {
            _firebaseClient = new FirebaseClient(
                "https://bookstore-497cc-default-rtdb.firebaseio.com/",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(_token)
                });
        }

        #region Genre

        public async Task<IList<Genre>> GetGenres()
        {
            var result = (await _firebaseClient
                .Child("genres")
                .OrderByKey()
                .OnceAsync<Genre>());
            return result is null
                ? new List<Genre>()
                : result.Select(x => x.Object).ToList();
        }

        public async Task<Genre> GetGenre(Guid requestGenreId)
        {
            return (await _firebaseClient
                .Child("genres")
                .OrderByKey()
                .EqualTo(requestGenreId.ToString)
                .OnceAsync<Genre>()).FirstOrDefault()?.Object;
        }

        public async Task AddGenre(Genre genre)
        {
            await _firebaseClient
                .Child("genres")
                .Child(genre.Id.ToString)
                .PutAsync(genre);
        }

        public async Task DeleteGenre(Guid requestGenreId)
        {
            await _firebaseClient
                .Child("genres")
                .Child(requestGenreId.ToString)
                .DeleteAsync();
        }

        #endregion

        #region Author

        public async Task AddAuthor(Author author)
        {
            await _firebaseClient
                .Child("authors")
                .Child(author.Id.ToString)
                .PutAsync(author);
        }

        public async Task DeleteAuthor(Guid requestId)
        {
            await _firebaseClient
                .Child("authors")
                .Child(requestId.ToString)
                .DeleteAsync();
        }

        public async Task<Author> GetAuthor(Guid authorId)
        {
            return (await _firebaseClient
                .Child("authors")
                .OrderByKey()
                .EqualTo(authorId.ToString)
                .OnceAsync<Author>()).FirstOrDefault()?.Object;
        }

        public async Task<IList<Author>> GetAuthors()
        {
            var result = (await _firebaseClient
                .Child("authors")
                .OrderByKey()
                .OnceAsync<Author>());
            return result is null
                ? new List<Author>()
                : result.Select(x => x.Object).ToList();
        }

        #endregion

        private async Task<string> GetToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(_apiKey.Value));
            var customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(Guid.NewGuid().ToString());
            return (await authProvider.SignInWithCustomTokenAsync(customToken)).FirebaseToken;
        }

        private void Authorize()
        {
            var path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            using (var stream =
                new FileStream($"{path}/bookstore-497cc-firebase-adminsdk-n7ikb-91821b33e8.json", FileMode.Open))
            {
                var options = new AppOptions
                {
                    Credential = GoogleCredential.FromStream(stream),
                    ServiceAccountId = Guid.NewGuid().ToString()
                };
                FirebaseApp.Create(options);
            }
        }

        // #region Add book
        //
        // public async Task AddBook(Book book, Author author, Genre genre)
        // {
        //     await PutPost(book, author, genre);
        //     await PutAuthor(author);
        //     await PutBook(book);
        // }
        //
        // private async Task PutBook(Book book)
        // {
        //     await _firebaseClient
        //         .Child("books")
        //         .Child(book.Id.ToString)
        //         .PutAsync(book);
        // }
        //
        // private async Task PutAuthor(Author author)
        // {
        //     await _firebaseClient
        //         .Child("authors")
        //         .Child(author.Id.ToString)
        //         .PutAsync(author);
        // }
        //
        // private async Task PutPost(Book book, Author author, Genre genre)
        // {
        //     await _firebaseClient
        //         .Child("genres")
        //         .Child(Enum.GetName(genre))
        //         .PostAsync(new Post()
        //         {
        //             AuthorId = author.Id.ToString(),
        //             BookId = book.Id.ToString()
        //         });
        // }
        //
        // #endregion

        // #region Get books
        //
        // public async Task<IEnumerable<Post>> GetPosts(Genre genre, int limit)
        // {
        //     return (await _firebaseClient
        //             .Child("genres")
        //             .Child(Enum.GetName(genre))
        //             .OrderByKey()
        //             .LimitToFirst(limit)
        //             .OnceAsync<Post>())
        //         .Select(p => p.Object);
        // }
        //
        // public async Task<IEnumerable<Book>> GetBooks(IEnumerable<Post> posts)
        // {
        //     var listOfBooks = new List<Book>();
        //     foreach (var post in posts)
        //     {
        //         var book = await _firebaseClient
        //             .Child("books")
        //             .OrderByKey()
        //             .StartAt(post.BookId)
        //             .OnceAsync<Book>();
        //         listOfBooks.Add(book.First().Object);
        //     }
        //
        //     return listOfBooks;
        // }
        //
        // #endregion
    }
}