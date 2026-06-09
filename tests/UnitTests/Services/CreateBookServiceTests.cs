using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MsBooks.Application.DTOs;
using MsBooks.Application.Exceptions;
using MsBooks.Application.Services;
using MsBooks.Application.Validators;
using MsBooks.Domain.Entities;
using MsBooks.Domain.Interfaces;
using Xunit;

namespace MsBooks.UnitTests.Services;

public class CreateBookServiceTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly CreateBookRequestValidator _validator;
    private readonly Mock<ILogger<CreateBookService>> _loggerMock;
    private readonly CreateBookService _service;

    public CreateBookServiceTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
        _validator = new CreateBookRequestValidator();
        _loggerMock = new Mock<ILogger<CreateBookService>>();
        _service = new CreateBookService(_repositoryMock.Object, _validator, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidRequest_ShouldCreateBookAndReturnResponse()
    {
        var request = new CreateBookRequest("Tradiciones peruanas", "Keiko Fujimori", "Libro de FP");

        var result = await _service.ExecuteAsync(request);

        result.Should().NotBeNull();
        result.Title.Should().Be("Tradiciones peruanas");
        result.Author.Should().Be("Keiko Fujimori");
        result.Description.Should().Be("Libro de FP");
        result.Id.Should().NotBeEmpty();
        result.Status.Should().Be("Disponible");

        _repositoryMock.Verify(r => r.AddAsync(
            It.IsAny<Book>(),
            It.IsAny<CancellationToken>()), Times.Once);

        _repositoryMock.Verify(r => r.SaveChangesAsync(
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyTitle_ShouldThrowValidationException()
    {
        var request = new CreateBookRequest("", "Keiko Fujimori", "Libro de FP");

        var act = async () => await _service.ExecuteAsync(request);

        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.And.Errors.Should().ContainKey("Title");
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyAuthor_ShouldThrowValidationException()
    {
        var request = new CreateBookRequest("Tradiciones peruana", "", "Libro de FP");

        var act = async () => await _service.ExecuteAsync(request);

        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.And.Errors.Should().ContainKey("Author");
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyDescription_ShouldThrowValidationException()
    {
        var request = new CreateBookRequest("Tradiciones peruanas", "Keiko Fujimori", "");

        var act = async () => await _service.ExecuteAsync(request);

        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.And.Errors.Should().ContainKey("Description");
    }
}
