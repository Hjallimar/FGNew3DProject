using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;


public class SpawnSystem : SystemBase
{
    public List<Entity> ActiveBullets;
    private List<Entity> UnusedBullets;
    protected override void OnUpdate()
    {
        var BulletSettings = GetSingleton<BulletSettings>();
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
    
    public void SpawnBullet()
    {
        if (UnusedBullets.Count > 0)
        {
            var Bullet = UnusedBullets[0];
           ActiveBullets.Add(Bullet);
           UnusedBullets.Remove(Bullet);
        }
    }
}