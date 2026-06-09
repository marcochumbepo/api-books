using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MsBooks.Application.Exceptions;
using MsBooks.Application.Services;
using MsBooks.Domain.Entities;
using MsBooks.Domain.Interfaces;
using Xunit;

namespace MsBooks.UnitTests.Services;

public class GetBookByIdServiceTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly Mock<ILogger<GetBookByIdService>> _loggerMock;
    private readonly GetBookByIdService _service;

    public GetBookByIdServiceTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
        _loggerMock = new Mock<ILogger<GetBookByIdService>>();
        _service = new GetBookByIdService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenBookExists_ShouldReturnBookResponse()
    {
        var bookId = Guid.NewGuid();
        var existingBook = Book.Create("Tradiciones peruanas", "Keiko Fujimori", "Libro de FP");

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBook);

        var result = await _service.ExecuteAsync(bookId);

        result.Should().NotBeNull();
        result.Title.Should().Be("Tradiciones peruanas");
        result.Author.Should().Be("Keiko Fujimori");
    }

    [Fact]
    public async Task ExecuteAsync_WhenBookDoesNotExist_ShouldThrowNotFoundException()
    {
        var bookId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        var act = async () => await _service.ExecuteAsync(bookId);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Libro con ID {bookId} no encontrado.");
    }
}
