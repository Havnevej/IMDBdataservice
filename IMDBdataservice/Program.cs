using IMDBdataservice.Service;
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

            BaseService bb = new BaseService();

            var x = bb.GetPerson("ZZ");
            foreach (var item in x)
            {
                Console.WriteLine(item.PersonName);
            }
        }
    }
}
