using System;
using MediatR;

namespace BookApi.Application.Contracts.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}