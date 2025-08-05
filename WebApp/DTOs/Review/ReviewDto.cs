namespace Frontend.DTOs.Review;

public class ReviewDto
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
}