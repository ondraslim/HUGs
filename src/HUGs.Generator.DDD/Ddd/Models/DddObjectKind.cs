using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HUGs.Generator.DDD.Ddd.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DddObjectKind
    {
        Entity,
        Aggregate,
        ValueObject,
        Enumeration
    }
}