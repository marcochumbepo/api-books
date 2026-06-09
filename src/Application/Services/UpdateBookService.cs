using Microsoft.Extensions.Logging;
using MsBooks.Application.DTOs;
using MsBooks.Application.Exceptions;
using MsBooks.Domain.Interfaces;

namespace MsBooks.Application.Services;

public class UpdateBookService
{
    private readonly IBookRepository _repository;
    private readonly ILogger<UpdateBookService> _logger;

    public UpdateBookService(IBookRepository repository, ILogger<UpdateBookService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<BookResponse> ExecuteAsync(Guid id, UpdateBookRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Actualizando libro con ID {BookId}", id);

        var book = await _repository.GetByIdAsync(id, cancellationToken);

        if (book is null)
        {
            _logger.LogWarning("Libro con ID {BookId} no encontrado para actualizar.", id);
            throw new NotFoundException($"Libro con ID {id} no encontrado.");
        }

        book.Update(request.Title, request.Author, request.Description, request.Status);

        await _repository.UpdateAsync(book, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Libro con ID {BookId} actualizado exitosamente.", id);

        return BookResponse.FromEntity(book);
    }
}
