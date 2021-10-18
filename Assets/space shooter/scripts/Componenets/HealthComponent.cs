using System;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct HealthComponent : IComponentData
{
    public float CurrentHealth;
    public float MaxHealth;
    public bool Dead;
}