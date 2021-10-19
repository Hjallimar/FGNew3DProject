using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class EnemySpawnSystem : SystemBase
{
    private float SpawnTimer = 0.0f;
    private float SpawnDelay;

    protected override void OnCreate()
    {
        EnemySettings enemySettings;

        if (!TryGetSingleton(out enemySettings))
        {
            SpawnDelay = 0.3f;
            return;
        }

        SpawnDelay = enemySettings.SpawnDelay;
        
        var EnemyInstances = new NativeArray<Entity>(enemySettings.Count, Allocator.Temp);
        EntityManager.Instantiate(enemySettings.Prefab, EnemyInstances);

        foreach (var e in EnemyInstances)
        {
            EntityManager.SetComponentData(e, new Disabled { });
        }

        EnemyInstances.Dispose();
    }

    protected override void OnUpdate()
    {
        SpawnTimer += Time.DeltaTime;

        if (SpawnTimer >= SpawnDelay)
            return;

        Entities
            .WithAny<EnemyTag, Disabled>()
            .ForEach((ref Entity entity, ref Translation translation) =>
        {
            EntityManager.RemoveComponent<Disabled>(entity);
            translation.Value = float3.zero;
        }).WithoutBurst().Run();
        SpawnTimer = 0.0f;
    }
}