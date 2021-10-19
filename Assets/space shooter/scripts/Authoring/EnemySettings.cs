using Unity.Entities;

// This attributes requires a MonoBehaviour mirroring the component struct to be generated.
// This way it can be attached to a GameObject, and at conversion the contents will be
// copied over to the corresponding IComponentData.
[GenerateAuthoringComponent]
public struct EnemySettings : IComponentData
{
    public Entity Prefab;
    public int    Count;
    public float  SpawnDelay;
}