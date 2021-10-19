
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform SpawnPoint;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Player goes pew pew");
            GameObject NewBullet = Instantiate(Bullet, transform);
            NewBullet.transform.parent = null;
            NewBullet.transform.position = SpawnPoint.position;
            // SpawnSystem Spawner = GetSingleton(SpawnSystem);
            // Spawner.SpawnBullet();
        }
    }
}
