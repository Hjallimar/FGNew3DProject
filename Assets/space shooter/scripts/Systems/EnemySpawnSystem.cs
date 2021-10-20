using Unity.Collections;
using Unity.Entities;

public class EnemySpawnSystem : SystemBase
{
    private EnemySettings EnemySettings;

    protected override void OnUpdate()
    {
        if (!TryGetSingleton(out EnemySettings))
        {
            Enabled = false;
            return;
        }
        
        var EnemyInstances = new NativeArray<Entity>(EnemySettings.Count, Allocator.Temp);
        
        var disabledEnemy = EnemySettings.Prefab;
        EntityManager.AddComponent<Disabled>(disabledEnemy);
        
        EntityManager.Instantiate(disabledEnemy, EnemyInstances);

        EnemyInstances.Dispose();

        Enabled = false;
    }
}