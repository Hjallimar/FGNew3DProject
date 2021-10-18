
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Entity Bullet;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Player goes pew pew");
            // SpawnSystem Spawner = GetSingleton(SpawnSystem);
            // Spawner.SpawnBullet();
        }
    }
}
