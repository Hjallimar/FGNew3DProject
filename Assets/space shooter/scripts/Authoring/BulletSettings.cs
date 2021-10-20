using Unity.Entities;
using UnityEngine;


[GenerateAuthoringComponent]
public struct BulletSettings : IComponentData
{
    public Entity Prefab;
}