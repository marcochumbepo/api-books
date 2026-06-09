using Microsoft.Extensions.Logging;
using MsBooks.Application.DTOs;
using MsBooks.Application.Exceptions;
using MsBooks.Domain.Interfaces;

namespace MsBooks.Application.Services;

public class GetBookByIdService
{
    private readonly IBookRepository _repository;
    private readonly ILogger<GetBookByIdService> _logger;

    public GetBookByIdService(IBookRepository repository, ILogger<GetBookByIdService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<BookResponse> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Buscando libro con ID {BookId}", id);

        var book = await _repository.GetByIdAsync(id, cancellationToken);

        if (book is null)
        {
            _logger.LogWarning("Libro con ID {BookId} no encontrado.", id);
            throw new NotFoundException($"Libro con ID {id} no encontrado.");
        }

        return BookResponse.FromEntity(book);
    }
}
