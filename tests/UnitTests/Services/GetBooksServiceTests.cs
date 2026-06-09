using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MsBooks.Application.Services;
using MsBooks.Domain.Entities;
using MsBooks.Domain.Interfaces;
using Xunit;

namespace MsBooks.UnitTests.Services;

public class GetBooksServiceTests
{
    private readonly Mock<IBookRepository> _repositoryMock;
    private readonly Mock<ILogger<GetBooksService>> _loggerMock;
    private readonly GetBooksService _service;

    public GetBooksServiceTests()
    {
        _repositoryMock = new Mock<IBookRepository>();
        _loggerMock = new Mock<ILogger<GetBooksService>>();
        _service = new GetBooksService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenBooksExist_ShouldReturnAllBooks()
    {
        var books = new List<Book>
        {
            Book.Create("Tradiciones Peruanas", "Keiko Fujimori", "Libro de FP"),
            Book.Create("La caida del chino", "Vargar Llosa", "Libro de la caida del chino")
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        var result = await _service.ExecuteAsync();
        var resultList = result.ToList();

        resultList.Should().HaveCount(2);
        resultList[0].Title.Should().Be("Tradiciones Peruanas");
        resultList[1].Author.Should().Be("Vargar Llosa");
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoBooksExist_ShouldReturnEmptyList()
    {
        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Book>());

        var result = await _service.ExecuteAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
