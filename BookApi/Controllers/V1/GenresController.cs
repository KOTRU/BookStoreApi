using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookApi.Application.Contracts.Genres.Commands.AddGenre;
using BookApi.Application.Contracts.Genres.Commands.DeleteGenre;
using BookApi.Application.Contracts.Genres.Queries.GetAllGenres;
using BookApi.Application.Contracts.Genres.Queries.GetGenre;
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
    public class GenresController : Controller
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public GenresController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all available genres
        /// </summary>
        /// <response code="200">Returns list of genres</response>
        /// <response code="400">Returns error if can't connect to db </response>
        [HttpGet(ApiRouts.Genres.GetAll)]
        [ProducesResponseType(typeof(IList<GetGenreResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var getAllGenresQuery = new GetAllGenresQuery();
            try
            {
                var genres = await _mediator.Send(getAllGenresQuery);
                var response = _mapper.Map<IList<Genre>, IList<GetGenreResponse>>(genres);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        /// <summary>
        /// Get genre by id
        /// </summary>
        /// <response code="200">Returns genre</response>
        /// <response code="400">Returns error if can't connect to db </response>
        [HttpGet(ApiRouts.Genres.Get)]
        [ProducesResponseType(typeof(GetGenreResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid genreId)
        {
            var getGenreQuery = new GetGenreQuery
            {
                Id = genreId
            };
            try
            {
                var genre = await _mediator.Send(getGenreQuery);
                if (genre is null) return NotFound();
                var response = _mapper.Map<Genre, GetGenreResponse>(genre);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        /// <summary>
        /// Add new genre
        /// </summary>
        /// <response code="201">Returns if genre successfully added</response>
        /// <response code="400">Returns error if can't connect to db </response>
        /// <response code="409">Returns if genre with that id already exists</response>
        [HttpPost(ApiRouts.Genres.Create)]
        [ProducesResponseType(typeof(CreateNewGenreResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateNewGenreRequest request)
        {
            if (request.Id.Equals(Guid.Empty))
                request.Id = Guid.NewGuid();
            var genreDto = _mapper.Map<CreateNewGenreRequest, Genre>(request);
            var addNewGenreCommand = new AddGenreCommand
            {
                Genre = genreDto
            };
            try
            {
                var created = await _mediator.Send(addNewGenreCommand);
                if (!created) return Conflict();
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
                var locationUtl = baseUrl + "/" + ApiRouts.Genres.Get.Replace("{genreId}", genreDto.Id.ToString());

                var createResponse = _mapper.Map<Genre, CreateNewGenreResponse>(genreDto);
                return Created(locationUtl, createResponse);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        /// <summary>
        /// Delete genre by id
        /// </summary>
        /// <response code="204">Returns if genre successfully deleted</response>
        /// <response code="404">Returns if genre not found</response>
        [HttpDelete(ApiRouts.Genres.Delete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid genreId)
        {
            var addNewGenreCommand = new DeleteGenreCommand
            {
                Id = genreId
            };
            try
            {
                var deleted = await _mediator.Send(addNewGenreCommand);
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