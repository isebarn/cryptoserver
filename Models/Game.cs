namespace Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }         
    }
}