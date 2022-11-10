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
    [Header("기본미니언")]
    public GameObject[] BasicMinion;
    
    [HideInInspector]
   public int BlueSkillWave;

    [HideInInspector]
    public int RedSkillWave;


    [HideInInspector]
    public bool skillOn;

    [HideInInspector]
    public string tag;

    
    float elaspedTime;
    float minionSpawnTime = 20f;

    void Start()
    {
        Turret.minionTowerEvent += ChangeMinion;
        BlueSpawnMinion();
        RedSpawnMinion();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"{BlueSkillWave} {RedSkillWave}"); 
        elaspedTime += Time.deltaTime;
        if (elaspedTime >= minionSpawnTime)
        {
            elaspedTime = 0;
            BlueSpawnMinion();
            RedSpawnMinion();
            if (tag == "Blue" && BlueSkillWave > 0)
            {
                BlueSpawnMinion();
                BlueSkillWave--;
            }
            else if (tag == "Red" && RedSkillWave > 0)
            {
                RedSpawnMinion();
                RedSkillWave--;
            }
        }
    }

    void ChangeMinion(GameObject transferedMinion, string tag)
    {
        if (transferedMinion.transform.GetChild(0).GetComponent<EnemySatatus>()._eminiomtype == EnemySatatus.EMINIOMTYPE.Nomal)
        {
            if (tag == "Blue")
            {
                BasicMinion[0] = transferedMinion; //  블루 근접미니언
            }
            else
            {
                BasicMinion[2] = transferedMinion; // 레드 근접미니언

            }
        }

        else
        {
            if (tag == "Red")
            {
                BasicMinion[1] = transferedMinion; // 블루 원거리 미니언
            }
            else
            {
                BasicMinion[3] = transferedMinion; // 레드 원거리 미니언
            }
        }

        Debug.Log(tag);

    }

    private void BlueSpawnMinion()
    {

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            if (BasicMinion[0] == null || BasicMinion[1] == null)
            {
                return;
            }
            PhotonNetwork.Instantiate(BasicMinion[0].name, GameManager.Instance.spawnPositions[0].position, Quaternion.identity); // 이게 바뀜

            PhotonNetwork.Instantiate(BasicMinion[1].name, GameManager.Instance.spawnPositions[0].position, Quaternion.identity);
        }

    }

    private void RedSpawnMinion()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            if (BasicMinion[2] == null || BasicMinion[3] == null)
            {
                return;
            }

            PhotonNetwork.Instantiate(BasicMinion[2].name, GameManager.Instance.spawnPositions[1].position, Quaternion.identity); // 이게 바뀜

            PhotonNetwork.Instantiate(BasicMinion[3].name, GameManager.Instance.spawnPositions[1].position, Quaternion.identity);
        }
    }
}