using PlatformsService.Dtos;

namespace E3.PlatformService.SyncDataServices.Http;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto dto);
}
