using HUGs.Generator.DDD.Common;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var yaml = File.ReadAllText("../../../AddressValueObject.dddschema");
            var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var schema = deserializer.Deserialize<DddObjectSchema>(yaml);
        }
    }
}
