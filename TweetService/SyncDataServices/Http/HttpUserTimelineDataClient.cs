using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TweetService.Dtos;

namespace TweetService.SyncDataServices.Http
{
    public class HttpUserTimelineDataClient : IUserTimelineDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public HttpUserTimelineDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendTweetToUserTimeline(TweetReadDto tweet)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(tweet),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync($"{_configuration["UserTimelineService"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to user timeline service was OK");
            }
            else
            {
                Console.WriteLine("--> Sync POST to user timeline service was not OK");
            }
        }
    }
}