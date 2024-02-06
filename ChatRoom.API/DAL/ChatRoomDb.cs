using ChatRoom.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.API.DAL
{
    public class ChatRoomDb : DbContext
    {
        public ChatRoomDb(DbContextOptions<ChatRoomDb> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatUser> ChatsUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatUser>()
                .HasKey(cu => new { cu.UserId, cu.ChatId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
