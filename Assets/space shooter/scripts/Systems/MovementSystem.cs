using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach(( ref Translation translation, in MoveComponent moveComp) =>
        {
            translation.Value += (float3)moveComp.CurrentVelocity * deltaTime;
        }).ScheduleParallel();
    }
}
