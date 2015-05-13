using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BartoszMilewski
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var moizedRandom = Module1.Memoize((int arg) => random.Next(arg));
            Console.WriteLine(moizedRandom(5));
            Console.WriteLine(moizedRandom(6));
            Console.WriteLine(moizedRandom(5));
            Console.ReadLine();
        }
    }
}
