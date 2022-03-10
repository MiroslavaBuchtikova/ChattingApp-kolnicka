using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.DbAccess
{
    public class ChatDbContext : IdentityDbContext<IdentityUser>
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {

        }

        public DbSet<ChatMessage> ChatMessages { get; set; }
    }

    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }
        
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
    }
}
