namespace ChatRoom.API.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public List<User> Participants { get; set; }
        public List<Message>? Messages { get; set; }
    }
}
