using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class KillSystem : JobComponentSystem
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
    struct KillJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<KillTag> KillGroup;
        public ComponentDataFromEntity<HealthComponent> HealthGroup;
        public ComponentDataFromEntity<DamageComponent> DamageGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity EntityA = collisionEvent.EntityA;
            Entity EntityB = collisionEvent.EntityB;

            bool AisHealth = HealthGroup.HasComponent(EntityA);
            bool BisHealth = HealthGroup.HasComponent(EntityB);
            bool AisKill = KillGroup.HasComponent(EntityA);
            bool BisKill = KillGroup.HasComponent(EntityB);
            bool AisDamage = DamageGroup.HasComponent(EntityA);
            bool BisDamage = DamageGroup.HasComponent(EntityB);

            if (AisKill)
            {
                if(BisHealth)
                    ModifyHealth(ref EntityB);
                else if(BisDamage)
                    ModifyBullet(ref EntityB);
            }
            else if (BisKill)
            {
                if(AisHealth)
                    ModifyHealth(ref EntityA);
                else if(AisDamage)
                    ModifyBullet(ref EntityA);
            }
        }

        private void ModifyHealth(ref Entity health)
        {
            HealthComponent modified = HealthGroup[health];
            modified.Dead = true;
            HealthGroup[health] = modified;
        }

        private void ModifyBullet(ref Entity Bullet)
        {
            DamageComponent modified = DamageGroup[Bullet];
            modified.Hit = true;
            DamageGroup[Bullet] = modified;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new KillJob();

        job.KillGroup = GetComponentDataFromEntity<KillTag>(true);
        job.HealthGroup = GetComponentDataFromEntity<HealthComponent>(false);
        job.DamageGroup = GetComponentDataFromEntity<DamageComponent>(false);
        
        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDependencies);
        jobHandle.Complete();
        
        return jobHandle;
    }
}
