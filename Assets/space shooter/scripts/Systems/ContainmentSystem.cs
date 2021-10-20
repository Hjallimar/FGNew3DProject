using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ContainmentSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.WithAll<PlayerTag>()
            .ForEach((ref Translation translation) =>
            {
                float3 temp = translation.Value;
                temp.x = Mathf.Clamp(temp.x, -8.0f, 8.0f);
                temp.y = Mathf.Clamp(temp.y, -4.0f, 4.0f);

                translation.Value = temp;

            }).ScheduleParallel();
    }
}