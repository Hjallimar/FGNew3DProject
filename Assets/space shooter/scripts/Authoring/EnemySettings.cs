using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;

// This attributes requires a MonoBehaviour mirroring the component struct to be generated.
// This way it can be attached to a GameObject, and at conversion the contents will be
// copied over to the corresponding IComponentData.
[GenerateAuthoringComponent]
public struct EnemySettings : IComponentData
{
    public Vector3 MinSpawnPos;
    public Vector3 MaxSpawnPos;
    public Entity Prefab;
    public int    Count;
    public float  SpawnDelay;
}