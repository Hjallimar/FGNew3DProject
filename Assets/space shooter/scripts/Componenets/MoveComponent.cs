using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct MoveComponent : IComponentData
{
    [HideInInspector] 
    public Vector3 CurrentVelocity;
    public float MaxSpeed;
    public float Acceleration;
}

