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

    public static MinionSpawner Instance;
    public MinionDatabaseList minionDB;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Turret_Minion.minionTowerEvent += ChangeMinion;
        BlueSpawnMinion();
        RedSpawnMinion();
    }

    // Update is called once per frame
    void Update()
    {

        //elaspedTime += Time.deltaTime;
        //if (elaspedTime >= minionSpawnTime)
        //{
        //    elaspedTime = 0;
        //    BlueSpawnMinion();
        //    RedSpawnMinion();
        //    if (tag == "Blue" && BlueSkillWave > 0)
        //    {
        //        BlueSpawnMinion();
        //        BlueSkillWave--;
        //    }
        //    else if (tag == "Red" && RedSkillWave > 0)
        //    {
        //        RedSpawnMinion();
        //        RedSkillWave--;
        //    }
        //}
    }
    // 블루                             // 레드 
    void ChangeMinion(GameObject transferedMinionBlue,GameObject transferedMinionRed,string tag) // 
    {
        if (transferedMinionBlue.transform.GetChild(0).GetComponent<EnemySatatus>()._eminontpye == EMINIONTYPE.Nomal) //첫번째 미니언으로 타입 설정 근접이면
        {
            if (tag == "Blue") //근접이고 태그가 블루면 
            {
                BasicMinion[0] = transferedMinionBlue; //  블루 근접미니언
            }
            else // 근접이고 태그가 레드면
            {
                BasicMinion[2] = transferedMinionRed; // 레드 근접미니언

            }
        }

        else // 원거리 이면서
        {
            if (tag == "Blue") // 원거리면서 태그가 블루면  
            {
                BasicMinion[1] = transferedMinionBlue; // 블루 원거리 미니언
            }
            else
            {
                BasicMinion[3] = transferedMinionRed; // 레드 원거리 미니언
            }
        }


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