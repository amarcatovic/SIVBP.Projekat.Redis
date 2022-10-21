namespace Projekat.Front.Dtos
{
    public class PostReadDto
    {
        public int Id { get; set; }
        public int? AcceptedAnswerId { get; set; }
        public int? AnswerCount { get; set; }
        public string Body { get; set; } = null!;
        public int? CommentCount { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Title { get; set; }
        public int ViewCount { get; set; }
    }
}
