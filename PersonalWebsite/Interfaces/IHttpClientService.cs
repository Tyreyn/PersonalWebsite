namespace PersonalWebsite.Interfaces
{
    public interface IHttpClientService
    {
        Task<string> GetAsync(string url);
    }
}
