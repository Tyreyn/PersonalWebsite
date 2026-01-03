using PersonalWebsite.Interfaces;

namespace PersonalWebsite.Services
{
    public class RepositoryCacheRefresherService(IRepositoryDownloaderService repositoryDownloaderService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await repositoryDownloaderService.DownloadAllRepositories();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to refresh cache: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromHours(23), stoppingToken);
            }
        }
    }
}