using Microsoft.EntityFrameworkCore;
using ToDoBackend;

namespace ToDoBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ToDoItem> Tasks { get; set; }
    }
}
