using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Async.DataAccess;
using Async.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Async
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            System.Net.ServicePointManager.DefaultConnectionLimit = Int32.MaxValue;
            var genres = new Genres(configuration);
            var types = new Types(configuration);
            var animes = new Animes(configuration);
            var animeGenres = new AnimeGenres(configuration);
            var animeStaging = new AnimeStaging(configuration);
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            try
            {
                var saves = animeStaging.Read().Select(async (animeAsync) =>
                {
                    var anime = await animeAsync;
                    await animes.Save(anime);
                    await animeGenres.Save(anime.Id, anime.GenreIds);
                });
                await Task.WhenAll(saves);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine($"Run time: {stopWatch.ElapsedMilliseconds} ms");
        }
    }
}
