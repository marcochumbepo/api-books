using MsBooks.Domain.Entities;

namespace MsBooks.Application.DTOs;

public record BookResponse(Guid Id, string Title, string Author, string Description, string Status)
{
    public static BookResponse FromEntity(Book book) =>
        new(book.Id, book.Title, book.Author, book.Description, book.Status.ToString());
}
