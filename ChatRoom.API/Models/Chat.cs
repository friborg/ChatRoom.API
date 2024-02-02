namespace ChatRoom.API.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public List<UserChat> Participants { get; set; } = new List<UserChat>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
