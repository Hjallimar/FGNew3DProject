using Unity.Collections;
using Unity.Entities;

public class EnemySpawnSystem : SystemBase
{
    // private float SpawnTimer = 0.0f;
    // private float SpawnDelay;

    private EnemySettings EnemySettings;

    protected override void OnCreate()
    {
        base.OnCreate();
        
        if (!TryGetSingleton(out EnemySettings))
        {
            Enabled = false;
            return;
        }
    }

    protected override void OnUpdate()
    {
        var EnemyInstances = new NativeArray<Entity>(EnemySettings.Count, Allocator.Temp);

        var updatedEnemy = EnemySettings.Prefab;
        EntityManager.AddComponent<Disabled>(updatedEnemy);
        
        EntityManager.Instantiate(updatedEnemy, EnemyInstances);

        EnemyInstances.Dispose();

        Enabled = false;
    }
}