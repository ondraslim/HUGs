using System;

namespace HUGs.Generator.DDD.Ddd.Models.Configuration
{
    /// <summary>
    /// Represents configuration of the DDD generator.
    /// </summary>
    public class DddGeneratorConfiguration
    {
        public DddObjectTargetNamespaces TargetNamespaces { get; set; } = new();
        public string[] AdditionalUsings { get; set; } = { };

        public string GetTargetNamespaceForKind(DddObjectKind kind)
            => kind switch
            {
                DddObjectKind.ValueObject => TargetNamespaces.ValueObject,
                DddObjectKind.Entity => TargetNamespaces.Entity,
                DddObjectKind.Aggregate => TargetNamespaces.Aggregate,
                DddObjectKind.Enumeration => TargetNamespaces.Enumeration,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };
    }
}