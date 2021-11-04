using System;

namespace IMDBdataservice
{
    class Program
    {
        static void Main(string[] args)
        {
            Title aTitle = new() { IsAdult = true, PrimaryTitle = "Husseins Sextape", RunTimeMinutes = 1 };

            Console.WriteLine(aTitle.ToString());
        }
    }
}
