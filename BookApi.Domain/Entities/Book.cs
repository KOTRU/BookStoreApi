using System;

namespace BookApi.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishingDate { get; set; }
        public float Price { get; set; }
        public float Rating { get; set; }
        public string Preview { get; set; }
    }
}