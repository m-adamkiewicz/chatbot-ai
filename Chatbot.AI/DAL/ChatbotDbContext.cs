using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Chatbot.AI.Api.DAL;

public class ChatbotDbContext(DbContextOptions<ChatbotDbContext> options) : DbContext(options)
{
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>().HasKey(x => x.Id);
        base.OnModelCreating(modelBuilder);
    }
}

public class Message
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Author Author { get; set; }
    public Reaction? Reaction { get; set; }
}

public enum Author
{
    Human = 1,
    AI = 2
}

public enum Reaction
{
    ThumbsUp = 1,
    ThumbsDown = 2
}