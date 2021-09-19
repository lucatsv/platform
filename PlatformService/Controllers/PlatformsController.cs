using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using AutoMapper;
using System.Collections.Generic;
using PlatformService.Dtos;
using System;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class PlatformsController : Controller
    {
        private IPlatformRepo platformRepo;
        private IMapper mapper;
        private ICommandDataClient commandDataClient;

        public PlatformsController(IPlatformRepo platformRepo, IMapper mapper, ICommandDataClient commandDataClient)
        {
            this.platformRepo = platformRepo;
            this.mapper = mapper;
            this.commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms() {

            Console.WriteLine("Getting all platforms");
            var platformsItems = platformRepo.GetAllPlatforms();

            return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platformsItems));
        }

        [HttpGet("{id}", Name= nameof(GetPlatformById) )]
        public ActionResult<PlatformReadDto> GetPlatformById([FromRoute]int id) {

            var platformItem = platformRepo.GetPlatformById(id);

            if(platformItem == null) {
                return NotFound();
            }
            else {
                return Ok(mapper.Map<PlatformReadDto>(platformItem));
            }
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatformAsync([FromBody]PlatformCreateDto platformCreateDto) {
            
            var platform = mapper.Map<Platform>(platformCreateDto);
            platformRepo.CreatePlatform(platform);
            platformRepo.SaveChanges();

            var platformReadDto = mapper.Map<PlatformReadDto>(platform);

            try {
                await commandDataClient.SendPlatformToCommand(platformReadDto);
            } catch (Exception e) {
                Console.WriteLine($"Could not send synchronously: {e.Message} {e.InnerException}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id}, platformReadDto);
        }
   
    }
}