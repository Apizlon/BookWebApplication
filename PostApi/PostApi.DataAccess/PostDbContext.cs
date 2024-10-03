using Microsoft.EntityFrameworkCore;
using PostApi.Core.Models;

namespace PostApi.DataAccess;

public class PostDbContext : DbContext
{
    public PostDbContext(DbContextOptions<PostDbContext> options) : base(options) { }

    public DbSet<Post> Posts { get; set; }
}