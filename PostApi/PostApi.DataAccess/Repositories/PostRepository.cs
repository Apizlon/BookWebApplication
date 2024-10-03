using Microsoft.EntityFrameworkCore;
using PostApi.Core.Models;
using PostApi.DataAccess.Interfaces;

namespace PostApi.DataAccess.Repositories;

public class PostRepository : IPostRepository
{
    private readonly PostDbContext _context;

    public PostRepository(PostDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddPostAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post.Id;
    }

    public async Task DeletePostAsync(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Post> GetPostAsync(int id)
    {
        return await _context.Posts.FindAsync(id);
    }

    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task UpdatePostAsync(int id, Post post)
    {
        var existingPost = await _context.Posts.FindAsync(id);
        if (existingPost != null)
        {
            existingPost.Name = post.Name;
            existingPost.Content = post.Content;
            existingPost.MentionedBooks = post.MentionedBooks;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> PostExistsAsync(int id)
    {
        return await _context.Posts.AnyAsync(u => u.Id == id);
    }
}