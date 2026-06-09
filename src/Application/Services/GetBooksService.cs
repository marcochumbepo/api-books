using Microsoft.Extensions.Logging;
using MsBooks.Application.DTOs;
using MsBooks.Domain.Interfaces;

namespace MsBooks.Application.Services;

public class GetBooksService
{
    private readonly IBookRepository _repository;
    private readonly ILogger<GetBooksService> _logger;

    public GetBooksService(IBookRepository repository, ILogger<GetBooksService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<BookResponse>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo todos los libros.");

        var books = await _repository.GetAllAsync(cancellationToken);

        return books.Select(BookResponse.FromEntity);
    }
}
