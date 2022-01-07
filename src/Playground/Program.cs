//using HUGs.DDD.Generated.Enumeration;
using System;
using HUGs.DDD.Generated.Entity;

namespace Playground
{
    internal class Program
    {
        private static void Main()
        {
            new Country(new CountryId(Guid.Empty), "asd");
        }
    }
}

