using Unity.Collections;
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

    protected override void OnCreate()
    {
       //  base.OnCreate();
       //  
       //  var enemyQueryDesc = new EntityQueryDesc
       //  {
       //      All     = new ComponentType[] { typeof(EnemyTag), typeof(Disabled) },
       //      Options = EntityQueryOptions.IncludeDisabled
       //  };
       // DisabledEntityQuery = GetEntityQuery(enemyQueryDesc);
        
    }

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
        float3 spawnpos = (float3)Vector3.Lerp(EnemySettings.MinSpawnPos, EnemySettings.MaxSpawnPos, t);
        //int rand = Random.Range(0, EnemySettings.SpawnPos.Length);
        Translation spawnTrans = new Translation{Value = spawnpos};
        
        EntityManager.SetComponentData(Enemy, spawnTrans);
        

        // NativeArray<Entity> disabledEnemies = new NativeArray<Entity>(1, Allocator.Temp);
        //
        // DisabledEntityQuery.ToEntityArray(disabledEnemies, Allocator.Temp);
        // if (disabledEnemies.Length <= 0)
        // {
        //     bool iExist = disabledEnemies[0] != Entity.Null;
        //     Debug.Log(disabledEnemies.Length + " " + iExist);
        //     disabledEnemies.Dispose();
        //     return;
        // }
        //
        // Entity spawningEnemy = disabledEnemies[0];
        // EntityManager.RemoveComponent<Disabled>(spawningEnemy);
        //
        // Translation spawnTranslation = new Translation
        // {
        //     Value = float3.zero
        // };
        // EntityManager.SetComponentData(spawningEnemy, spawnTranslation);
        //
        // disabledEnemies.Dispose();
    }
}
