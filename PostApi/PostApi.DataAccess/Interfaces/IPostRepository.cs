using PostApi.Core.Models;

namespace PostApi.DataAccess.Interfaces;

public interface IPostRepository
{
    Task<int> AddPostAsync(Post post);
    Task DeletePostAsync(int id);
    Task<Post> GetPostAsync(int id);
    Task<IEnumerable<Post>> GetPostsAsync();
    Task UpdatePostAsync(int id, Post post);
    Task<bool> PostExistsAsync(int id);
}