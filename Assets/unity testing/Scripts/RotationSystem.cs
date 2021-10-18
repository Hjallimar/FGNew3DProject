using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RotationSystem : SystemBase
{
    private float timer = 0;
    protected override void OnUpdate()
    {
        var offset = math.radians((float)Time.ElapsedTime * 90);
        var factor = math.sin(offset) / 10;

        // Entities.ForEach is the default way of iterating over entities for processing.
        // // Notice the ScheduleParallel at the end, this will run over multiple threads.
        // Entities.ForEach((ref Rotation rotation, in Translation translation) =>
        // {
        //     rotation.Value = quaternion.RotateX(offset + translation.Value.x * factor);
        // }).ScheduleParallel();

        // #region Player Movement Input
        // Vector3 Direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f);
        // Entities.ForEach(( ref MoveComponent MoveComp, in PlayerTag playerTag) =>
        // {
        //     MoveComp.CurrentVelocity = Direction.normalized * playerTag.Speed;
        // }).ScheduleParallel();
        // #endregion

        #region Movement Handler
        float deltaTime = Time.DeltaTime;
        Entities.ForEach(( ref Translation translation, in MoveComponent MoveComp) =>
        {
            translation.Value += (float3)MoveComp.CurrentVelocity  * deltaTime;
        }).ScheduleParallel();
        #endregion

        #region Dammage Handler
        Entities.ForEach(( ref HealthComponent HealthComp, in DamageTag DmgTag) =>
        {
            HealthComp.CurrentHealth -= DmgTag.Damage;
            // Debug.Log("I take damage, have: " + HealthComp.CurrentHealth + " left");
            //Destroy(DmgTag);

        }).ScheduleParallel();
        #endregion
 
    }
}