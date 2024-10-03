using PostApi.Application.Contracts;
using PostApi.Core.Models;

namespace PostApi.Application.Mappers;

public static class PostMapper
{
    public static Post MapToCore(this PostRequest request)
    {
        return new Post
        {
            Name = request.Name,
            Content = request.Content,
            MentionedBooks = request.MentionedBooks
        };
    }

    public static PostResponse MapToContract(this Post post)
    {
        return new PostResponse
        {
            Id = post.Id,
            Name = post.Name,
            Content = post.Content,
            MentionedBooks = post.MentionedBooks
        };
    }

    public static IEnumerable<PostResponse> MapToContract(this IEnumerable<Post> posts)
    {
        return posts.Select(x => x.MapToContract());
    }
}