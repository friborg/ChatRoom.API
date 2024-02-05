namespace ChatRoom.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Chat> Chats { get; set; } = new List<Chat>();
    }
}
