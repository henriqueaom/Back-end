using Microsoft.EntityFrameworkCore;
namespace Contatos.Models
{
    public class ApplicationDbContext : DbContext
{
    public DbSet<Reminders> Reminder { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=Lembrete.sqlite");
}
}

