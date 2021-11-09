using IMDBdataservice.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IMDBdataservice
{
    class Program
    {
        static async Task Main(string[] args)
        {
            imdbContext imdb = new imdbContext();
            BaseService bb = new BaseService();
            var t = await bb.SeeRatingOfMovie("tt0057345");
            foreach(var item in t)
            {
                Console.WriteLine(item.PrimaryTitle + item.titlerating.RatingAvg);
            }


        }
    }
}
