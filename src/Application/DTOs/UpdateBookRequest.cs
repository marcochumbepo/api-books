namespace MsBooks.Application.DTOs;

public record UpdateBookRequest(string? Title, string? Author, string? Description, string? Status);
