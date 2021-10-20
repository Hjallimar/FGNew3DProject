using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;


public class SpawnSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // SpawnBullets();
        // SpawnEnemies();

        /*var BulletInstances = new NativeArray<Entity>(BulletSettings.Count, Allocator.Temp);
        EntityManager.Instantiate(BulletInstances.prefab, BulletSettings);
        for (int i = 0; i < BulletInstances.Length; i++)
        {
            EntityManager.SetComponentData(BulletInstances[i], new MoveComponent {Velocity = Vector3.up * ((i +1) * 5.0f)});
            UnusedBullets.Add(BulletInstances[i]);
        }
        BulletInstances.Dispose();*/
        Enabled = false;
    }

    private void SpawnEnemies()
    {
        var EnemySettings = GetSingleton<EnemySettings>();

        var EnemyInstances = new NativeArray<Entity>(EnemySettings.Count, Allocator.Temp);
        EntityManager.Instantiate(EnemySettings.Prefab, EnemyInstances);

        EnemyInstances.Dispose();
    }
}