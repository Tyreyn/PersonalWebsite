using PersonalWebsite.Models.DataObjects;

namespace PersonalWebsite.Interfaces
{
    public interface IRepositoryDownloaderService
    {
        Task<List<Repositories>?> DownloadAllRepositories();

        Task<string?> DownloadReadme(string projectName);

        Task<IList<string>> DownloadLanguages(string url);
    }
}