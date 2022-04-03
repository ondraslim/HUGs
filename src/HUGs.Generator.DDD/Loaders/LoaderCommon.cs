using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HUGs.Generator.DDD.Ddd.Loaders
{
    /// <summary>
    /// Common utilities for loaders.
    /// </summary>
    public static class LoaderCommon
    {
        private static readonly IDeserializer Deserializer = new DeserializerBuilder()
            .WithNamingConvention(NullNamingConvention.Instance)
            .Build();

        /// <summary>
        /// Deserializers YAML text to type with the common deserializer instance.
        /// </summary>
        public static T Deserialize<T>(string text)
        {
            return Deserializer.Deserialize<T>(text);
        }
    }
}