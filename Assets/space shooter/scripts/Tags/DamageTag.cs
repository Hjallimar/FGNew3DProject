using Unity.Entities;

[GenerateAuthoringComponent]
public struct DamageTag : IComponentData
{
    public float Damage;
    public bool Hit;
}
