using Microsoft.EntityFrameworkCore;
using UserService.Controllers.Models;

namespace UserService.Controllers.Data;

public class UserDbContext: DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}