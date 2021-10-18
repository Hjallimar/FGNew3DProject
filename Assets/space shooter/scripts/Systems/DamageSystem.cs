using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

public class DamageSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    [BurstCompile]
    struct DamageJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<DamageTag> DamageGroup;
        public ComponentDataFromEntity<HealthComponent> HealthGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity EntityA = collisionEvent.EntityA;
            Entity EntityB = collisionEvent.EntityB;

            if (HealthGroup.HasComponent(EntityA) && DamageGroup.HasComponent(EntityB))
            {
                Modify(ref EntityA, ref EntityB);
            }
            else if (HealthGroup.HasComponent(EntityB) && DamageGroup.HasComponent(EntityA))
            {
                Modify(ref EntityB, ref EntityA);
            }
        }

        private void Modify(ref Entity health, ref Entity damageTag)
        {
            HealthComponent modified = HealthGroup[health];
            DamageTag damage = DamageGroup[damageTag];
            modified.CurrentHealth -= damage.Damage;
            if (modified.CurrentHealth <= 0)
            {
                modified.Dead = true;
            }
            HealthGroup[health] = modified;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new DamageJob();

        job.DamageGroup = GetComponentDataFromEntity<DamageTag>(true);
        job.HealthGroup = GetComponentDataFromEntity<HealthComponent>(false);

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld,
            inputDependencies);
        jobHandle.Complete();
        
        return jobHandle;

    }
}
