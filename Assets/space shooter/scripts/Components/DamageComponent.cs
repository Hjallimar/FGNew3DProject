using Unity.Entities;

[GenerateAuthoringComponent]
public struct DamageComponent : IComponentData
{
    public float Damage;
    public bool Hit;
}
