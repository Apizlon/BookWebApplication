namespace PostApi.Application.Contracts;

public class PostRequest
{
    public string Name { get; set; }
    public string Content { get; set; }
    public List<int> MentionedBooks { get; set; }
}