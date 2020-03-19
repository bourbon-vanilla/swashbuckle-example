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
    [Route("api/v{version:apiVersion}/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorsRepository;
        private readonly IMapper _mapper;

        public AuthorsController(
            IAuthorRepository authorsRepository,
            IMapper mapper)
        {
            _authorsRepository = authorsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authorsFromRepo = await _authorsRepository.GetAuthorsAsync();
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }

        [HttpGet("{authorId}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(
            Guid authorId)
        {
            var authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }

        [HttpPut("{authorId}")]
        public async Task<ActionResult<AuthorDto>> UpdateAuthor(
            Guid authorId,
            AuthorForUpdateDto authorForUpdate)
        {
            var authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(authorForUpdate, authorFromRepo);

            //// update & save
            _authorsRepository.UpdateAuthor(authorFromRepo);
            await _authorsRepository.SaveChangesAsync();

            // return the author
            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }

        [HttpPatch("{authorId}")]
        public async Task<ActionResult<AuthorDto>> UpdateAuthor(
            Guid authorId,
            JsonPatchDocument<AuthorForUpdateDto> patchDocument)
        {
            var authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();
            }

            // map to DTO to apply the patch to
            var author = _mapper.Map<AuthorForUpdateDto>(authorFromRepo);
            //patchDocument.ApplyTo(author, ModelState);
            patchDocument.ApplyTo(author);

            // if there are errors when applying the patch the patch doc 
            // was badly formed  These aren't caught via the ApiController
            // validation, so we must manually check the modelstate and
            // potentially return these errors.
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            // map the applied changes on the DTO back into the entity
            _mapper.Map(author, authorFromRepo);

            // update & save
            _authorsRepository.UpdateAuthor(authorFromRepo);
            await _authorsRepository.SaveChangesAsync();

            // return the author
            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }
    }
}
