namespace ChatRoom.API.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public User SentFrom { get; set; }
        public DateTime TimeStamp { get; set; }
    }   
}
