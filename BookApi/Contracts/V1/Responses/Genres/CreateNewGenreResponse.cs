using System;
using BookApi.Application.Common.Mappings;
using BookApi.Domain.Entities;

namespace BookApi.Contracts.V1.Responses
{
    public class CreateNewGenreResponse:IMapFrom<Genre>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}