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
            var res = await bb.GetMostFrequentPerson("nm0000016");
            var c = res.Count();
            var g = res.GroupBy(i => i).OrderByDescending(x => x.Count()).Select(x => x.Key);
            var count = res.GroupBy(i => i).OrderByDescending(x => x.Count()).Select(x => x.Count()).ToList();
            var i = 0;
            Console.WriteLine(g);
            foreach (var item in g)
            {
                Console.WriteLine(item.PersonName + " " + count[i]);
                i++;
            }
            /*
            foreach (var item in res)
            {
                Console.WriteLine(c);
            }*/
        }
    }
}
