namespace ServerMVCProject.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int AdminId { get; set; }
    }
}