using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PlatformService.Data
{
    public static class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder app) {
            using (var serviceScope = app.ApplicationServices.CreateScope()) {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context) {
            if(context.Platforms.Any()) {
                Console.WriteLine("DB already contains data");
            }
            else {
                Console.WriteLine("Seeding data");

                context.Platforms.AddRange(
                    new Models.Platform() {
                        Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"
                    },
                    new Models.Platform() {
                        Name = "Postgress", Publisher = "Postgres", Cost = "Free"
                    },
                    new Models.Platform() {
                        Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}