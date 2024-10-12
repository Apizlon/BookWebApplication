using MyBookApp.Application.Contracts;

namespace MyBookApp.Application.Interfaces;

public interface IBookService
{
    public Task StartConsumingAsync();
    Task<int> AddBookAsync(BookRequest bookRequest);
    Task DeleteBookAsync(int id);
    Task<BookResponse> GetBookAsync(int id);
    Task UpdateBookAsync(int id, BookRequest bookRequest);
    Task<bool> BookExistsAsync(int id);
}