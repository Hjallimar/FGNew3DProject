using System;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct HealthComponent : IComponentData
{
    public float CurrentHealth;
    public float MaxHealth;
    public bool Dead;

    public HealthComponent(float health)
    {
        CurrentHealth = health; 
        MaxHealth = health;
        Dead = false;
    }
}