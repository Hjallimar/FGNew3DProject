using Unity.Entities;

public class EnemyGradualSpawner : SystemBase
{
    private float SpawnTimer = 0.0f;
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
        SpawnTimer += Time.DeltaTime;

        if (SpawnTimer < EnemySettings.SpawnDelay)
            return;

        // var enemyQueryDesc = new EntityQueryDesc
        // {
        //     All = new ComponentType[]{ typeof(EnemyTag), () }
        // }
        // Entities.WithAny<EnemyTag, Disabled>().ForEach(() =>
        // {
        //     
        // })


    }
}
