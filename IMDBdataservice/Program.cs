using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace IMDBdataservice
{
    class Program
    {
        static void Main(string[] args)
        {
            imdbContext imdb = new imdbContext();
            Title aTitle = new() { IsAdult = true, PrimaryTitle = "Husseins Sextape", RunTimeMinutes = 1 };

            var x = imdb.Titles.ToListAsync().Result;
            foreach (var item in x)
            {
                Console.WriteLine(item.PrimaryTitle);
            }
        }
    }
}
