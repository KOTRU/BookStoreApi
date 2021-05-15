using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BookApi.Application.Common.Mappings;
using BookApi.Domain.Entities;

namespace BookApi.Contracts.V1.Requests
{
    public class CreateNewAuthorRequest : IMapFrom<Author>
    {
        public Guid Id { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public DateTime BirthYear { get; set; }
        public DateTime? DeathYear { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateNewAuthorRequest, Author>();
        }
    }
}