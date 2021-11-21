using HUGs.DDD.Generated.Enumeration;
using System;

namespace Playground
{
    internal class Program
    {
        private static void Main()
        {
            //var x = HUGs.DDD.Generated.Enumeration.OrderState.Created;
            //Console.WriteLine(x);

            var orderState = OrderState.Created;
            Console.WriteLine(orderState.ToString());
        }
    }
}

