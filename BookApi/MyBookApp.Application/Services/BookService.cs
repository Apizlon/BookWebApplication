using Confluent.Kafka;
using MyBookApp.Application.Contracts;
using MyBookApp.Application.Interfaces;
using MyBookApp.Application.Kafka;
using MyBookApp.Application.Mappers;
using MyBookApp.Application.Validators;
using MyBookApp.Core.Exceptions;
using MyBookApp.Core.Models;
using MyBookApp.DataAccess.Interfaces;
using Newtonsoft.Json;

namespace MyBookApp.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IKafkaProducer _kafkaProducer;
    public BookService(IBookRepository bookRepository, IKafkaProducer kafkaProducer)
    {
        _bookRepository = bookRepository;
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task StartConsumingAsync()
    {
        var config = new ConsumerConfig
        {
            GroupId = "new-book-group",
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("book-requests");
        try
        {
            while (true)
            {
                Console.WriteLine("Waiting for message...");
                var consumeResult = consumer.Consume();
                Console.WriteLine("got em");
                var bookRequest = JsonConvert.DeserializeObject<MyBookApp.Application.Kafka.BookRequest>(consumeResult.Message.Value);
                Console.WriteLine("3");
                if (bookRequest?.BookIds != null && bookRequest.BookIds.Any())
                {
                    // Retrieve books based on IDs
                    Console.WriteLine("4");
                    List<BookResponse> books = new List<BookResponse>();
                    foreach (var id in bookRequest.BookIds)
                    {
                        var book = await GetBookAsync(id);
                        books.Add(book);
                    }
                    Console.WriteLine("5");

                    // Send book responses back via Kafka
                    await _kafkaProducer.SendMessageAsync("book-responses", JsonConvert.SerializeObject(books));
                    Console.WriteLine("6");
                }
            }
        }
        catch (ConsumeException ex)
        {
            // Handle exceptions
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }
    
    public async Task<int> AddBookAsync(Contracts.BookRequest bookRequest)
    {
        bookRequest.AddValidation();
        return await _bookRepository.AddBookAsync(bookRequest.MapToCore());
    }

    public async Task DeleteBookAsync(int id)
    {
        var isBookExists = await BookExistsAsync(id);
        if (!isBookExists)
        {
            throw new BookNotFoundException(id);
        }
        await _bookRepository.DeleteBookAsync(id);
    }

    public async Task<BookResponse> GetBookAsync(int id)
    {
        var isBookExists = await BookExistsAsync(id);
        if (!isBookExists)
        {
            throw new BookNotFoundException(id);
        }
        var book = await _bookRepository.GetBookAsync(id);
        return book.MapToContract();
    }

    public async Task UpdateBookAsync(int id, Contracts.BookRequest bookRequest)
    {
        var isBookExists = await BookExistsAsync(id);
        bookRequest.UpdateValidation(id,isBookExists);
        await _bookRepository.UpdateBookAsync(id, bookRequest.MapToCore());
    }

    public async Task<bool> BookExistsAsync(int id)
    {
        return await _bookRepository.BookExistsAsync(id);
    }
}