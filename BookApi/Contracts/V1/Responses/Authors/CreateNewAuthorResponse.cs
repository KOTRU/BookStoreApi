using System;
using BookApi.Application.Common.Mappings;
using BookApi.Domain.Entities;

namespace BookApi.Contracts.V1.Responses
{
    public class CreateNewAuthorResponse : IMapFrom<Author>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthYear { get; set; }
        public DateTime? DeathYear { get; set; }
    }
}