using System;
using BookApi.Domain.Entities;
using MediatR;

namespace BookApi.Application.Contracts.Authors.Queries.GetAuthor
{
    public class GetAuthorQuery:IRequest<Author>
    {
        public Guid Id { get; set; }
    }
}