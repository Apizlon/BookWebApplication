namespace PostApi.Core.Models;

public class Post
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public List<int> MentionedBooks { get; set; }
}