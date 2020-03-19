using AutoMapper;
using SwashbuckleExample.Services;
using Microsoft.AspNetCore.Mvc;
using SwashbuckleExample.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SwashbuckleExample.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/authors/{authorId}/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public BooksController(
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(Guid authorId)
        {
            if (!await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            var booksFromRepo = await _bookRepository.GetBooksAsync(authorId);
            return Ok(_mapper.Map<IEnumerable<BookDto>>(booksFromRepo));
        }

        /// <summary>
        /// Get a book by id for a specific author.
        /// </summary>
        /// <param name="authorId">The id of the book author.</param>
        /// <param name="bookId">The id of the book.</param>
        /// <returns>An ActionResult of type Book.</returns>
        /// <response code="200">Returns the requested book.</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{bookId}")]
        public async Task<ActionResult<BookDto>> GetBook(Guid authorId, Guid bookId)
        {
            if (!await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            var bookFromRepo = await _bookRepository.GetBookAsync(authorId, bookId);
            if (bookFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookDto>(bookFromRepo));
        }


        [HttpPost()]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity,
            Type = typeof(ModelStateDictionary))]
        public async Task<ActionResult<BookDto>> CreateBook(
            Guid authorId,
            [FromBody] BookForCreationDto bookForCreation)
        {
            if (!await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            var bookToAdd = _mapper.Map<Entities.Book>(bookForCreation);
            _bookRepository.AddBook(bookToAdd);
            await _bookRepository.SaveChangesAsync();

            return CreatedAtRoute(
                "GetBook",
                new { authorId, bookId = bookToAdd.Id },
                _mapper.Map<BookDto>(bookToAdd));
        }
    }
}
