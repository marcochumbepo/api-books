using FluentValidation;
using Microsoft.Extensions.Logging;
using MsBooks.Application.DTOs;
using MsBooks.Application.Validators;
using MsBooks.Domain.Entities;
using MsBooks.Domain.Interfaces;

namespace MsBooks.Application.Services;

public class CreateBookService
{
    private readonly IBookRepository _repository;
    private readonly CreateBookRequestValidator _validator;
    private readonly ILogger<CreateBookService> _logger;

    public CreateBookService(
        IBookRepository repository,
        CreateBookRequestValidator validator,
        ILogger<CreateBookService> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<BookResponse> ExecuteAsync(CreateBookRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            _logger.LogWarning("Validación fallida al crear libro: {@Errors}", errors);
            throw new Exceptions.ValidationException("Error de validación al crear libro.", errors);
        }

        var book = Book.Create(request.Title, request.Author, request.Description);

        await _repository.AddAsync(book, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Libro creado exitosamente con ID {BookId}", book.Id);

        return BookResponse.FromEntity(book);
    }
}
