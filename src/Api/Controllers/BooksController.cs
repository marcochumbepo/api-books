using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsBooks.Application.DTOs;
using MsBooks.Application.Services;

namespace MsBooks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly CreateBookService _createBookService;
    private readonly GetBooksService _getBooksService;
    private readonly GetBookByIdService _getBookByIdService;
    private readonly UpdateBookService _updateBookService;

    public BooksController(
        CreateBookService createBookService,
        GetBooksService getBooksService,
        GetBookByIdService getBookByIdService,
        UpdateBookService updateBookService)
    {
        _createBookService = createBookService;
        _getBooksService = getBooksService;
        _getBookByIdService = getBookByIdService;
        _updateBookService = updateBookService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookResponse>> Create(
        [FromBody] CreateBookRequest request,
        CancellationToken cancellationToken)
    {
        var book = await _createBookService.ExecuteAsync(request, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var books = await _getBooksService.ExecuteAsync(cancellationToken);
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var book = await _getBookByIdService.ExecuteAsync(id, cancellationToken);
        return Ok(book);
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookResponse>> Update(
        Guid id,
        [FromBody] UpdateBookRequest request,
        CancellationToken cancellationToken)
    {
        var book = await _updateBookService.ExecuteAsync(id, request, cancellationToken);
        return Ok(book);
    }
}
