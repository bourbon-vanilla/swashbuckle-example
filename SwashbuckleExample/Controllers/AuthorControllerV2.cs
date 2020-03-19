using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SwashbuckleExample.Dto;
using SwashbuckleExample.Services;


namespace SwashbuckleExample.Controllers
{

    [Produces("application/json", "application/xml")]
    [ApiController]
    [Route("api/v{version:apiVersion}/authors")]
    [ApiVersion("2.0")]
    public class AuthorsControllerV2 : ControllerBase
    {
        private readonly IAuthorRepository _authorsRepository;
        private readonly IMapper _mapper;

        public AuthorsControllerV2(
            IAuthorRepository authorsRepository,
            IMapper mapper)
        {
            _authorsRepository = authorsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the authors (V2)
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of Author</returns>
        /// <response code="200">Returns the list of authors</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authorsFromRepo = await _authorsRepository.GetAuthorsAsync();
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }
    }
}
