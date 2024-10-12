using PostApi.Application.Kafka;

namespace PostApi.Application.Contracts;

public class PostResponseWithBooks
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public List<BookResponse> MentionedBooks { get; set; }
}