using MyBookApp.Application.Contracts;
using MyBookApp.Core.Models;

namespace MyBookApp.Application.Mappers;

public static class BookMapper
{
    public static Book MapToCore(this BookRequest request)
    {
        return new Book
        {
            Name = request.Name,
            Description = request.Description,
            Author = request.Author
        };
    }

    public static BookResponse MapToContract(this Book book)
    {
        return new BookResponse
        {
            Id = book.Id,
            Name = book.Name,
            Description = book.Description,
            Author = book.Author
        };
    }

    public static IEnumerable<BookResponse> MapToContract(this IEnumerable<Book> books)
    {
        return books.Select(x => x.MapToContract());
    }
}