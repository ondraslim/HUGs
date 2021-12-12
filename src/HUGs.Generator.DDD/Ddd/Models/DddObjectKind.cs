using System.Text.Json.Serialization;

namespace HUGs.Generator.DDD.Ddd.Models
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