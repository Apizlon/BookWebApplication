namespace PostApi.Application.Contracts;

public class PostResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public List<int> MentionedBooks { get; set; }
}