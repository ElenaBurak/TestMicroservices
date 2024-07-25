using System.Text;
using System.Text.Json;
using PlatformsService.Dtos;

namespace E3.PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendPlatformToCommand(PlatformReadDto platformDto)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platformDto),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("--> Sync POST to CommandService was OK!");
        }
        else
        {
            Console.WriteLine("--> Sync POST to CommandService was FAIL!");
        }
    }
}
