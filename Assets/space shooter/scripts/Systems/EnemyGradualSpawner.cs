using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyGradualSpawner : SystemBase
{
    private float SpawnTimer = 0.0f;
    private EnemySettings EnemySettings;

    private EntityQuery DisabledEntityQuery;

    protected override void OnUpdate()
    {
        if (!TryGetSingleton(out EnemySettings))
        {
            Enabled = false;
            return;
        }
        
        SpawnTimer += Time.DeltaTime;
        
        if (SpawnTimer < EnemySettings.SpawnDelay)
            return;

        SpawnTimer = 0.0f;

        Entity Enemy = EntityManager.Instantiate(EnemySettings.Prefab);
        float t = Random.Range(0.0f, 1.0f);
        float3 spawnpos = Vector3.Lerp(EnemySettings.MinSpawnPos, EnemySettings.MaxSpawnPos, t);
        Translation spawnTrans = new Translation{Value = spawnpos};
        
        EntityManager.SetComponentData(Enemy, spawnTrans);
    }
}
