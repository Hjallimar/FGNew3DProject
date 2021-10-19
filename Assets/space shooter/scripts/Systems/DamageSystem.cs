using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
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
        public ComponentDataFromEntity<DamageTag> DamageGroup;
        public ComponentDataFromEntity<HealthComponent> HealthGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity EntityA = collisionEvent.EntityA;
            Entity EntityB = collisionEvent.EntityB;

            bool AisHealth = HealthGroup.HasComponent(EntityA);
            bool BisHealth = HealthGroup.HasComponent(EntityB);
            bool AisDamage = DamageGroup.HasComponent(EntityA);
            bool BisDamage = DamageGroup.HasComponent(EntityB);
            
            if (AisHealth && BisDamage)
            {
                Modify(ref EntityA, ref EntityB);
            }
            else if (AisDamage && BisHealth)
            {
                Modify(ref EntityB, ref EntityA);
            }
        }

        private void Modify(ref Entity health, ref Entity damageTag)
        {
            
            DamageTag damage = DamageGroup[damageTag];
            if(damage.Hit)
                return;
            HealthComponent modified = HealthGroup[health];
            modified.CurrentHealth -= damage.Damage;
            if (modified.CurrentHealth <= 0)
            {
                modified.Dead = true;
            }
            damage.Hit = true;
            HealthGroup[health] = modified;
            DamageGroup[damageTag] = damage;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new DamageJob();

        job.DamageGroup = GetComponentDataFromEntity<DamageTag>(false);
        job.HealthGroup = GetComponentDataFromEntity<HealthComponent>(false);

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDependencies);
        jobHandle.Complete();
        
        return jobHandle;
    }
}
