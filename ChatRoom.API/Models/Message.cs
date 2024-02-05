namespace ChatRoom.API.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }   
}
