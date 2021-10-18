using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnSystem : SystemBase
{
    private int priviet = 0;
    
    protected override void OnCreate()
    {
        var enemySettings  = GetSingleton<EnemySettings>();
        var EnemyInstances = new NativeArray<Entity>(enemySettings.Count, Allocator.Temp);
        EntityManager.Instantiate(enemySettings.Prefab, EnemyInstances);
        
        Debug.Log("JAG KAN SKRIVA HÄR ;)");

        priviet++;
        foreach (var e in EnemyInstances)
        {
            // EntityManager.SetComponentData(e, new Disabled{});
        }
        
        Debug.Log("JAG KAN SKRIVA HÄR OCKSÅ ;)");

        priviet++;
        
        EnemyInstances.Dispose();
        Debug.Log("JAG KAN SKRIVA HÄR ASDFASDF ;)");
        
        priviet++;
        int i = 0;
    }

    protected override void OnUpdate()
    {
       
        // Enabled = false;
    }
}
