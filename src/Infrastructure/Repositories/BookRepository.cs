using Microsoft.EntityFrameworkCore;
using MsBooks.Domain.Entities;
using MsBooks.Domain.Interfaces;
using MsBooks.Infrastructure.Data;

namespace MsBooks.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .AsNoTracking()
            .OrderByDescending(b => b.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        await _context.Books.AddAsync(book, cancellationToken);
    }

    public async Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        _context.Books.Update(book);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
