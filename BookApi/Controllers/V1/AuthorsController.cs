using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookApi.Application.Contracts.Authors.Commands.CreateNewAuthor;
using BookApi.Application.Contracts.Authors.Commands.DeleteAuthor;
using BookApi.Application.Contracts.Authors.Queries.GetAllAuthors;
using BookApi.Application.Contracts.Authors.Queries.GetAuthor;
using BookApi.Contracts.V1;
using BookApi.Contracts.V1.Requests;
using BookApi.Contracts.V1.Responses;
using BookApi.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers.V1
{
    /// <summary>
    /// Genre controller
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    public class AuthorsController : Controller
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AuthorsController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all available authors
        /// </summary>
        /// <response code="200">Returns list of genres</response>
        /// <response code="400">Returns error if can't connect to db </response>
        [HttpGet(ApiRouts.Authors.GetAll)]
        [ProducesResponseType(typeof(IList<GetAuthorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var getAllAuthorsQuery = new GetAllAuthorsQuery();
            try
            {
                var authors = await _mediator.Send(getAllAuthorsQuery);
                var response = _mapper.Map<IList<Author>, IList<GetAuthorResponse>>(authors);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        /// <summary>
        /// Get author by id
        /// </summary>
        /// <response code="200">Returns author</response>
        /// <response code="400">Returns error if can't connect to db </response>
        /// <response code="404">Returns if author not found</response>
        [HttpGet(ApiRouts.Authors.Get)]
        [ProducesResponseType(typeof(GetAuthorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid authorId)
        {
            var getAuthorQuery = new GetAuthorQuery
            {
                Id = authorId
            };
            try
            {
                var author = await _mediator.Send(getAuthorQuery);
                if (author is null) return NotFound();
                var response = _mapper.Map<Author, GetAuthorResponse>(author);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        /// <summary>
        /// Add new author
        /// </summary>
        /// <response code="201">Returns if author successfully added</response>
        /// <response code="400">Returns error if can't connect to db </response>
        /// <response code="409">Returns if author with that id already exists</response>
        [HttpPost(ApiRouts.Authors.Create)]
        [ProducesResponseType(typeof(CreateNewAuthorResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateNewAuthorRequest request)
        {
            if (request.Id.Equals(Guid.Empty))
                request.Id = Guid.NewGuid();
            var authorDto = _mapper.Map<CreateNewAuthorRequest, Author>(request);
            var createAuthorCommand = new CreateNewAuthorCommand
            {
                Author = authorDto
            };
            try
            {
                var created = await _mediator.Send(createAuthorCommand);
                if (!created) return Conflict();
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
                var locationUtl = baseUrl + "/" + ApiRouts.Genres.Get.Replace("{authorId}", authorDto.Id.ToString());

                var createResponse = _mapper.Map<Author, CreateNewAuthorResponse>(authorDto);
                return Created(locationUtl, createResponse);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        /// <summary>
        /// Delete author by id
        /// </summary>
        /// <response code="204">Returns if author successfully deleted</response>
        /// <response code="404">Returns if author not found</response>
        [HttpDelete(ApiRouts.Authors.Delete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid authorId)
        {
            var deleteAuthorCommand = new DeleteAuthorCommand
            {
                Id = authorId
            };
            try
            {
                var deleted = await _mediator.Send(deleteAuthorCommand);
                if (deleted)
                    return NoContent();
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}