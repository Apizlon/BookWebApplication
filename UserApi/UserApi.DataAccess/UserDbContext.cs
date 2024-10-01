using Microsoft.EntityFrameworkCore;
using UserApi.Core.Models;

namespace UserApi.DataAccess;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}