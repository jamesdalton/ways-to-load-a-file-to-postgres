using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ARowAtATime.DataAccess;
using ARowAtATime.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;


namespace ARowAtATime
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
                foreach (var anime in animeStaging.Read())
                {
                    animes.Save(anime);
                    animeGenres.Save(anime.Id, anime.GenreIds);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine($"Run time: {stopWatch.ElapsedMilliseconds} ms");
        }
    }
}
