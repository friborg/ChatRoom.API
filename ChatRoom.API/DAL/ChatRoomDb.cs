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
    }
}
