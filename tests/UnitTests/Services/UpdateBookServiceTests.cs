using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MsBooks.Application.DTOs;
using MsBooks.Application.Exceptions;
using MsBooks.Application.Services;
using MsBooks.Domain.Entities;
using MsBooks.Domain.Interfaces;
using Xunit;

namespace MsBooks.UnitTests.Services;

public class UpdateBookServiceTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly Mock<ILogger<UpdateBookService>> _loggerMock;
    private readonly UpdateBookService _service;

    public UpdateBookServiceTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
        _loggerMock = new Mock<ILogger<UpdateBookService>>();
        _service = new UpdateBookService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenBookExists_ShouldUpdateAndReturnResponse()
    {
        var bookId = Guid.NewGuid();
        var existingBook = Book.Create("Días de soledad", "Gabriel García", "Novela clásica");

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBook);

        var request = new UpdateBookRequest("El amor en tiempos de cólera", "Gabriel García", null, "Prestado");

        var result = await _service.ExecuteAsync(bookId, request);

        result.Should().NotBeNull();
        result.Title.Should().Be("El amor en tiempos de cólera");
        result.Author.Should().Be("Gabriel García");
        result.Description.Should().Be("Novela clásica");
        result.Status.Should().Be("Prestado");

        _repositoryMock.Verify(r => r.UpdateAsync(existingBook, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenBookDoesNotExist_ShouldThrowNotFoundException()
    {
        var bookId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        var request = new UpdateBookRequest("Título", null, null, null);

        var act = async () => await _service.ExecuteAsync(bookId, request);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Libro con ID {bookId} no encontrado.");
    }

    [Fact]
    public async Task ExecuteAsync_WithPartialUpdate_ShouldOnlyUpdateProvidedFields()
    {
        var bookId = Guid.NewGuid();
        var existingBook = Book.Create("El túnel", "Marco Chumbe", "Novela");

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBook);

        var request = new UpdateBookRequest(null, null, "Descripción actualizada", null);

        var result = await _service.ExecuteAsync(bookId, request);

        result.Title.Should().Be("El túnel");
        result.Author.Should().Be("Marco Chumbe");
        result.Description.Should().Be("Descripción actualizada");
        result.Status.Should().Be("Disponible");
    }
}
