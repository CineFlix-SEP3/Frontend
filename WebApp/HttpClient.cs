using System.Net.Http;

namespace Frontend;

public class HttpClient
{
    private readonly System.Net.Http.HttpClient _client = new()
    {
        BaseAddress = new Uri("https://localhost:7086")
    };
}