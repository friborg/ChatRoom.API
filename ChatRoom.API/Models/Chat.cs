using System.Text.Json.Serialization;

namespace ChatRoom.API.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
        public List<User> Participants { get; set; } = new List<User>();
    }
}
