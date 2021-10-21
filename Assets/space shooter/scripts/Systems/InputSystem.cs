using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class InputSystem : SystemBase
{
    private float                                  TimeSinceLastBullet = 0.0f;
    private float                                  Cooldown                  = 0.4f;
    private BulletSettings                         BulletSettings;
    private EndSimulationEntityCommandBufferSystem CommandBufferSystem;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        CommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        GatherMovementInput(deltaTime);
        
        TimeSinceLastBullet += deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Space) && TimeSinceLastBullet > Cooldown)
        {
            TimeSinceLastBullet = 0.0f;
       
            Fire();
        }
    }

    private void GatherMovementInput(float deltaTime)
    {
        Vector3 Direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f).normalized;
        bool    DirectionMagnitudeIsApproximatelyZero = Mathf.Approximately(Direction.magnitude, 0);
        
        Entities
            .WithAll<PlayerTag>()
            .ForEach((ref MoveComponent moveComp) =>
            {
                if (DirectionMagnitudeIsApproximatelyZero)
                {
                    moveComp.CurrentVelocity *= 0.95f;
                }
                else
                {
                    moveComp.CurrentVelocity += (moveComp.Acceleration * deltaTime) * Direction;
                    float dotVelocity = Vector3.Dot(moveComp.CurrentVelocity, Direction);
                    if (dotVelocity >= moveComp.MaxSpeed)
                    {
                        moveComp.CurrentVelocity = moveComp.CurrentVelocity.normalized * moveComp.MaxSpeed;
                    }
                }
            }).Schedule();
    }

    private void Fire()
    {
        if (!TryGetSingleton(out BulletSettings))
        {
            return;
        }
        EntityCommandBuffer entityCommandBuffer = CommandBufferSystem.CreateCommandBuffer();


        float3 spawnPos = default;
        Entities
            .WithAll<PlayerFirepointTag>()
            .ForEach((in LocalToWorld localToWorld) =>
            {
                spawnPos = localToWorld.Position;
            }).Run();
    
        
        Entity Bullet = BulletSettings.Prefab;
        Translation spawnTranslation = new Translation
        {
            Value = spawnPos
        };
        
        EntityManager.SetComponentData(Bullet, spawnTranslation);
        entityCommandBuffer.Instantiate(Bullet);
        
        CommandBufferSystem.AddJobHandleForProducer(this.Dependency);
    }
}
