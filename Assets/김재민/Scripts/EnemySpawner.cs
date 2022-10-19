using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class EnemySpawner : MonoBehaviourPun
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
    GameObject EnemyPrefabs; // 근접
    [SerializeField]
    GameObject ShotEnemyPrefabs; // 원거리

    Vector3 spawnPos = new Vector3(55.55f, 1.06f, 0f);


    int EnemyCount = 1;
    
    void Start()
    { 
       

        for (int i = 0; i < EnemyCount; i++)

        {

            GameObject PistonEnemy = Instantiate(EnemyPrefabs, spawnPos, transform.rotation);
            GameObject ShotEnemy = Instantiate(ShotEnemyPrefabs, spawnPos, transform.rotation);

        }



    }

   
   
    
}
