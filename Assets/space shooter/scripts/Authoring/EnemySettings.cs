using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct EnemySettings : IComponentData
{
    public Vector3 MinSpawnPos;
    public Vector3 MaxSpawnPos;
    public Entity Prefab;
    public int    Count;
    public float  SpawnDelay;
}