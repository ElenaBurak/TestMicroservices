using AutoMapper;
using Azure.Core.Pipeline;
using E3.PlatformService.SyncDataServices.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using PlatformsService.Data;
using PlatformsService.Dtos;
using PlatformsService.Models;

namespace PlatformsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepo repo, IMapper mapper, ICommandDataClient commandDataClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms...");
            var platformItems = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repo.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var model = _mapper.Map<Platform>(platformCreateDto);
            _repo.CreatePlatform(model);
            _repo.SaveChanges();

            var dto = _mapper.Map<PlatformReadDto>(model);

            try
            {
                await _commandDataClient.SendPlatformToCommand(dto);
            }
            catch (Exception ex) { Console.WriteLine($"--> Could not send synchronously: {ex.Message}"); }

            return CreatedAtRoute(nameof(GetPlatformById), new { id = dto.Id }, dto);

        }
    }
}