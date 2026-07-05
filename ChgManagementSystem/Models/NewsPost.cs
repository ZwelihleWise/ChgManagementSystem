namespace ChgManagementSystem.Models
{
    public class NewsPost
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Category { get; set; }

        public string? ImagePath { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.Now;

        public DateTime? EventDate { get; set; }

        public bool IsPublished { get; set; } = true;
    }
}
