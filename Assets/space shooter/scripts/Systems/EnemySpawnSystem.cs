using Unity.Collections;
using Unity.Entities;

public class EnemySpawnSystem : SystemBase
{
    protected override void OnCreate()
    {
        EnemySettings enemySettings;
        if (!TryGetSingleton(out enemySettings))
        {
            Enabled = false;
            return;
        }

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
        // Enabled = false;
    }
}