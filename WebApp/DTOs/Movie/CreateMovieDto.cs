namespace Frontend.DTOs.Movie;

public class CreateMovieDto
{
    public string Title { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = new();
    public List<string> Directors { get; set; } = new();
    public List<string> Actors { get; set; } = new();
    public int RunTime { get; set; }
    public string ReleaseDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string PosterUrl { get; set; } = string.Empty;
}