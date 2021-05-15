using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BookApi.Application.Common.Mappings;
using BookApi.Domain.Entities;

namespace BookApi.Contracts.V1.Requests
{
    public class CreateNewGenreRequest : IMapFrom<Genre>
    {
        public Guid Id { get; set; }
        [Required] public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateNewGenreRequest, Genre>();
        }
    }
}