using System;

namespace BookApi.Domain.Entities
{
    public class Post
    {
        public Guid AuthorId { get; set; }
        public Guid BookId { get; set; }
    }
}