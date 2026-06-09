namespace MsBooks.Domain.Entities;

public class Book
{
    public Guid Id { get; private set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Description { get; set; } = null!;
    public BookStatus Status { get; set; }

    private Book() { }

    public static Book Create(string title, string author, string description)
    {
        return new Book
        {
            Id = Guid.NewGuid(),
            Title = title,
            Author = author,
            Description = description,
            Status = BookStatus.Disponible
        };
    }

    public void Update(string? title, string? author, string? description, string? status)
    {
        if (title is not null) Title = title;
        if (author is not null) Author = author;
        if (description is not null) Description = description;
        if (status is not null && Enum.TryParse<BookStatus>(status, true, out var parsedStatus))
            Status = parsedStatus;
    }
}
