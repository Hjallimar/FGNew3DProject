using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnSystem : SystemBase
{
    
    
    protected override void OnCreate()
    {
        var EnemySettings  = GetSingleton<EnemySettings>();
        var EnemyInstances = new NativeArray<Entity>(EnemySettings.Count, Allocator.Temp);
        EntityManager.Instantiate(EnemySettings.Prefab, EnemyInstances);

        foreach (var e in EnemyInstances)
        {
            // EntityManager.SetComponentData(e, new Disabled{});
        }

        EnemyInstances.Dispose();
    }

    protected override void OnUpdate()
    {
       
        // Enabled = false;
    }
}
