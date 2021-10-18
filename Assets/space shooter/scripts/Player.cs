
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Range(1.0f, 10.0f)] private float MoveSpeed = 10.0f;
    [SerializeField] private Entity Bullet;
    [SerializeField] private float Speed = 10.0f;
    private Vector3 InputDirection = Vector3.zero;
    private MoveComponent MoveComp;
    
    void Start()
    {
        MoveComp = GetComponent<MoveComponent>();
    }

    
    void Update()
    {
        InputDirection.x = Input.GetAxisRaw("Horizontal");
        InputDirection.y = Input.GetAxisRaw("Vertical");
        MoveComp.Velocity = InputDirection * Speed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // SpawnSystem Spawner = GetSingleton(SpawnSystem);
            // Spawner.SpawnBullet();
        }
    }
}
