using PostApi.Application.Contracts;
using PostApi.Application.Interfaces;
using PostApi.Application.Mappers;
using PostApi.Core.Exceptions;
using PostApi.DataAccess.Interfaces;

namespace PostApi.Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
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

    public async Task<PostResponse> GetPostAsync(int id)
    {
        var isPostExists = await PostExistsAsync(id);
        if (!isPostExists)
        {
            throw new PostNotFoundException(id);
        }
        var post = await _postRepository.GetPostAsync(id);
        return post.MapToContract();
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