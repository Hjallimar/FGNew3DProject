using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach(( ref Translation translation, in MoveComponent moveComp) =>
        {
            translation.Value += (float3)moveComp.CurrentVelocity * deltaTime;
        }).ScheduleParallel();
        
        Entities.ForEach(( ref Translation translation, ref Rotation rotation, ref ForwardMovementComponent moveComp) =>
        {
            Vector3 forward = math.forward(rotation.Value);
            moveComp.CurrentVelocity += (moveComp.Acceleration * deltaTime) * forward;
            float dotVelocity = Vector3.Dot(moveComp.CurrentVelocity, forward);
            if (dotVelocity >= moveComp.MaxSpeed)
            {
                moveComp.CurrentVelocity = moveComp.CurrentVelocity.normalized * moveComp.MaxSpeed;
            }
            translation.Value += (float3)moveComp.CurrentVelocity * deltaTime;
        }).ScheduleParallel();
    }
}
