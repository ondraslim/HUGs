using System.Text.Json.Serialization;

namespace HUGs.Generator.DDD.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DddObjectKind
    {
        Entity,
        Aggregate,
        ValueObject,
        Enumeration
    }
}