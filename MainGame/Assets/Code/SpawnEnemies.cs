using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    //Outlets
    public GameObject[] enemies;
    public Vector3[] spawnPositions;
    public bool spawned;
    
    
    public void SpawnEnemy()
    {
        
        if (enemies.Length > 0)
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                if (!spawned)
                {
                    Instantiate(enemies[i],spawnPositions[i], transform.rotation);
                }
            }
            
            spawned = true;
        }
    }
}
