using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner1 : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    public enum ENUMZOMBIETYPE
    { 
        Normal,
        Shot,
        Special,
    }
    
    [SerializeField]
    GameObject EnemyPrefabs1; // 근접
    [SerializeField]
    GameObject ShotEnemyPrefabs2; // 원거리

    Vector3 spawnPos1 = new Vector3(-55.09f, 1.06f, 0f);


    int EnemyCount1 = 1;
    
    void Start()
    { 
       

        for (int i = 0; i < EnemyCount1; i++)

        {

            GameObject PistonEnemy = Instantiate(EnemyPrefabs1, spawnPos1, transform.rotation);
            GameObject ShotEnemy = Instantiate(ShotEnemyPrefabs2, spawnPos1, transform.rotation);

        }



    }

   
   
    
}
