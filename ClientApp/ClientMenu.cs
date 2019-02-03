using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientApp
{
    public class ClientMenu
    {
        public static int Menu()
        {
            int retVal = 0;
            Console.WriteLine("Choose action:");
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Read");
            Console.WriteLine("3. Edit");
            Console.WriteLine("4. Delete");
            Console.WriteLine("5. Calculate");
            Console.WriteLine("6. Exit");
            retVal = int.Parse(Console.ReadLine());

            return retVal;
        }
    }
}
