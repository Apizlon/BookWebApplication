using System.Collections.Concurrent;
using Newtonsoft.Json;
using PostApi.Application.Contracts;
using PostApi.Application.Interfaces;
using PostApi.Application.Kafka;
using PostApi.Application.Mappers;
using PostApi.Core.Exceptions;
using PostApi.DataAccess.Interfaces;

namespace PostApi.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly IKafkaConsumer _kafkaConsumer;

    public PostService(IPostRepository postRepository, IKafkaProducer kafkaProducer, IKafkaConsumer kafkaConsumer)
    {
        _postRepository = postRepository;
        _kafkaProducer = kafkaProducer;
        _kafkaConsumer = kafkaConsumer;
    }

    public async Task<int> AddPostAsync(PostRequest postRequest)
    {
        return await _postRepository.AddPostAsync(postRequest.MapToCore());
    }

    public async Task DeletePostAsync(int id)
    {
        var isPostExists = await PostExistsAsync(id);
        if (!isPostExists)
        {
            throw new PostNotFoundException(id);
        }
        await _postRepository.DeletePostAsync(id);
    }

    public async Task<PostResponseWithBooks> GetPostAsync(int id)
    {
        // 1. Проверяем, существует ли пост
        var isPostExists = await PostExistsAsync(id);
        if (!isPostExists)
        {
            throw new PostNotFoundException(id);
        }

        // 2. Получаем пост из репозитория
        var post = await _postRepository.GetPostAsync(id);
        var postResponse = post.MapToContract();

        // 3. Собираем уникальные идентификаторы книг из поста
        var mentionedBookIds = postResponse.MentionedBooks.Distinct().ToList();

        // 4. Создаем запрос для получения книг
        var bookRequest = new BookRequest
        {
            BookIds = mentionedBookIds.ToArray()
        };

        var booksResponse = new ConcurrentBag<BookResponse>();
        var cancellationTokenSource = new CancellationTokenSource();
        
        // Запускаем Kafka Consumer для получения ответов от микросервиса книг
        _ = Task.Run(() =>
        {
            _kafkaConsumer.ConsumeMessagesAsync("book-responses", message =>
            {
                Console.WriteLine(message);
                booksResponse = JsonConvert.DeserializeObject<ConcurrentBag<BookResponse>>(message);
            }, cancellationTokenSource.Token);
        });

        // 6. Отправляем запрос на получение книг через Kafka
        await _kafkaProducer.SendMessageAsync("book-requests", JsonConvert.SerializeObject(bookRequest));
        Console.WriteLine("sended");
        // Ждем некоторое время для получения ответов (например, 5 секунд)
        await Task.Delay(5000);

        // Завершаем потребление
        cancellationTokenSource.Cancel();

        // 7. Обрабатываем ответы и обновляем пост с книгами
        Console.WriteLine(BagToString(booksResponse));
        PostResponseWithBooks postResponseWithBooks = new PostResponseWithBooks
        {
            Id = postResponse.Id,
            Name = postResponse.Name,
            Content = postResponse.Content,
            MentionedBooks = booksResponse.Where(b => b != null).ToList()
        };

        return postResponseWithBooks;
    }

    private string BagToString(ConcurrentBag<BookResponse> bag)
    {
        string res = "";
        foreach (var item in bag)
        {
            res += $"{item.Id} {item.Name} {item.Author} {item.Description} \n";
        }

        return res;
    }

    public async Task<IEnumerable<PostResponse>> GetPostsAsync()
    {
        var posts = await _postRepository.GetPostsAsync();
        return posts.MapToContract();
    }

    public async Task UpdatePostAsync(int id, PostRequest postRequest)
    {
        var isPostExists = await PostExistsAsync(id);
        if (!isPostExists)
        {
            throw new PostNotFoundException(id);
        }
        await _postRepository.UpdatePostAsync(id, postRequest.MapToCore());
    }

    public async Task<bool> PostExistsAsync(int id)
    {
        return await _postRepository.PostExistsAsync(id);
    }
}