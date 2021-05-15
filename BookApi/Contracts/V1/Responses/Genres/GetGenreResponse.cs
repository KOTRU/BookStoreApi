using System;
using AutoMapper;
using BookApi.Application.Common.Mappings;
using BookApi.Domain.Entities;

namespace BookApi.Contracts.V1.Responses
{
    public class GetGenreResponse : IMapFrom<Genre>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Genre, GetGenreResponse>();
        }
    }
}