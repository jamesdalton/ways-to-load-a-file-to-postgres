using System;
using System.Collections.Generic;
using System.Linq;
using ReadAllWriteAll.DataAccess;
using ReadAllWriteAll.Models;
using Npgsql;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ReadAllWriteAll
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            var genres = new Genres(configuration);
            var types = new Types(configuration);
            var animes = new Animes(configuration);
            var animeGenres = new AnimeGenres(configuration);
            var animeStaging = new AnimeStaging(configuration);
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            try
            {
                var anime = animeStaging.Read();
                animes.Save(anime);
                animeGenres.Save(anime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine($"Run time: {stopWatch.ElapsedMilliseconds} ms");
        }
    }
}
