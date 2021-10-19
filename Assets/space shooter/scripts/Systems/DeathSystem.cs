using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public class DeathSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        Entities.ForEach((Entity entity, in DamageTag damageTag) =>
        {
            if (damageTag.Hit)
            {
                entityCommandBuffer.DestroyEntity(entity);
            }
        }).Schedule();
        
        Entities.ForEach((Entity entity, in HealthComponent healthComponent) =>
        {
            if (healthComponent.Dead)
            {
                entityCommandBuffer.DestroyEntity(entity);
            }
        }).Schedule();

        commandBufferSystem.AddJobHandleForProducer(this.Dependency);
    }
}
