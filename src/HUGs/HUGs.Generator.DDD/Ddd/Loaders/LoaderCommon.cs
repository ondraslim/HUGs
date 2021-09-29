using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HUGs.Generator.DDD.Ddd.Loaders
{
    public static class LoaderCommon
    {
        private static readonly IDeserializer Deserializer = new DeserializerBuilder()
            .WithNamingConvention(NullNamingConvention.Instance)
            .Build();

        public static T Deserialize<T>(string text)
        {
            return Deserializer.Deserialize<T>(text);
        }
    }
}