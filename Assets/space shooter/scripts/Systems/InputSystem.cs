using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class InputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Assign values to local variables captured in your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     float deltaTime = Time.DeltaTime;

        // This declares a new kind of job, which is a unit of work to do.
        // The job is declared as an Entities.ForEach with the target components as parameters,
        // meaning it will process all entities in the world that have both
        // Translation and Rotation components. Change it to process the component
        // types you want.

        float deltaTime = Time.DeltaTime;
        
        Vector3 Direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f).normalized;
        
        Entities
            .WithAll<PlayerTag>()
            .ForEach((ref MoveComponent moveComp) =>
        {
            float dotVelocity = Vector3.Dot(moveComp.CurrentVelocity, Direction);
            if (dotVelocity < moveComp.MaxSpeed)
            {
                moveComp.CurrentVelocity += (moveComp.Acceleration * deltaTime) * Direction;
            }
        }).Schedule();
    }
}
