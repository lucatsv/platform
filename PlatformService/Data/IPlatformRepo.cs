using System.Collections.Generic;
using PlatformService.Models;
namespace PlatformService.Data
{
    public interface IPlatformRepo {
        void CreatePlatform(Platform platform);
        Platform GetPlatformById(int id);
        IEnumerable<Platform> GetAllPlatforms();        
        bool SaveChanges();
    }
}