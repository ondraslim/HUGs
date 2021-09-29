using System;
using System.Collections.Generic;

namespace HUGs.Generator.DDD.Ddd.Models.Configuration
{
    public class DddGeneratorConfiguration
    {
        public DddObjectTargetNamespaces TargetNamespaces { get; set; } = new();
        public ICollection<string> AdditionalUsings { get; set; } = new List<string>();


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