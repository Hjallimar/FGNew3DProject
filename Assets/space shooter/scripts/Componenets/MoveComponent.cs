using System;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MoveComponent : IComponentData
{
    public float3 Velocity;
}
