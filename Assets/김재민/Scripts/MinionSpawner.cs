using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MinionSpawner : MonoBehaviourPunCallbacks
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    [Header("NomalMinion")]
    public GameObject[] EnemyPrefabs;


    float elaspedTime;
    float minionSpawnTime = 20f;

    void Start()
    {
        Turret.minionTowerEvent += ChangeMinion;
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        elaspedTime += Time.deltaTime;
        if (elaspedTime >= minionSpawnTime)
        {
            elaspedTime = 0;
            SpawnEnemy();
        }
    }

    void ChangeMinion(GameObject transferedMinion, string tag)
    {
        if(transferedMinion.transform.GetChild(0).GetComponent<EnemySatatus>()._eminiomtype == EnemySatatus.EMINIOMTYPE.Nomal)
        {
            if (tag == "Blue")
            {
                EnemyPrefabs[0] = transferedMinion;
            }
            else
            {
                EnemyPrefabs[2] = transferedMinion;

            }
        }

        else
        {
            if (tag == "Red")
            {
                EnemyPrefabs[1] = transferedMinion;
            }
            else
            {
                EnemyPrefabs[3] = transferedMinion;
            }
        }

    }

    private void SpawnEnemy()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            if (EnemyPrefabs[0] == null || EnemyPrefabs[1] == null)
            {
                return;
            }

            PhotonNetwork.Instantiate(EnemyPrefabs[0].name, GameManager.Instance.spawnPositions[0].position, Quaternion.identity); // ¿Ã∞‘ πŸ≤Ò

            PhotonNetwork.Instantiate(EnemyPrefabs[1].name, GameManager.Instance.spawnPositions[0].position, Quaternion.identity);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            if (EnemyPrefabs[2] == null || EnemyPrefabs[3] == null)
            {
                return;
            }
            PhotonNetwork.Instantiate(EnemyPrefabs[2].name, GameManager.Instance.spawnPositions[1].position, Quaternion.identity); // ¿Ã∞‘ πŸ≤Ò

            PhotonNetwork.Instantiate(EnemyPrefabs[3].name, GameManager.Instance.spawnPositions[1].position, Quaternion.identity);
        }
    }

}
