using PostApi.Application.Contracts;

namespace PostApi.Application.Interfaces;

public interface IPostService
{
    Task<int> AddPostAsync(PostRequest postRequest);
    Task DeletePostAsync(int id);
    Task<PostResponseWithBooks> GetPostAsync(int id);
    Task<IEnumerable<PostResponse>> GetPostsAsync();
    Task UpdatePostAsync(int id, PostRequest postRequest);
    Task<bool> PostExistsAsync(int id);
}