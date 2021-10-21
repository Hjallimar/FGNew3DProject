using Unity.Entities;


[GenerateAuthoringComponent]
public struct BulletSettings : IComponentData
{
    public Entity Prefab;
}