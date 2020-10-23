namespace Models
{
    public class EmailUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }   
        public bool Developer { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }         
    }
}