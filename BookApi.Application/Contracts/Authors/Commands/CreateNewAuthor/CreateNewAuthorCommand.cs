using BookApi.Domain.Entities;
using MediatR;

namespace BookApi.Application.Contracts.Authors.Commands.CreateNewAuthor
{
    public class CreateNewAuthorCommand:IRequest<bool>
    {
        public Author Author { get; set; }
    }
}