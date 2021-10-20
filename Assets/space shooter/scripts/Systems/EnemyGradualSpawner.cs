using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class EnemyGradualSpawner : SystemBase
{
    private float SpawnTimer = 0.0f;
    private EnemySettings EnemySettings;

    private EntityQuery DisabledEntityQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        
        var enemyQueryDesc = new EntityQueryDesc
        {
            All     = new ComponentType[] { typeof(EnemyTag), typeof(Disabled) },
            Options = EntityQueryOptions.IncludeDisabled
        };
       DisabledEntityQuery = GetEntityQuery(enemyQueryDesc);
        
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
        

        NativeArray<Entity> disabledEnemies = new NativeArray<Entity>(1, Allocator.Temp);

        DisabledEntityQuery.ToEntityArray(disabledEnemies, Allocator.Temp);
        if (disabledEnemies.Length <= 0 || disabledEnemies[0] == Entity.Null)
        {
            bool iExist = disabledEnemies[0] != Entity.Null;
            Debug.Log(disabledEnemies.Length + " " + iExist);
            disabledEnemies.Dispose();
            return;
        }
        Entity spawningEnemy = disabledEnemies[0];
        EntityManager.RemoveComponent<Disabled>(spawningEnemy);

        Translation spawnTranslation = new Translation
        {
            Value = float3.zero
        };
        EntityManager.SetComponentData(spawningEnemy, spawnTranslation);

        disabledEnemies.Dispose();
    }
}
