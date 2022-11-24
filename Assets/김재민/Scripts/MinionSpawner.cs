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
    [Header("�⺻�̴Ͼ�")]
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
    // ���                             // ���� 
    void ChangeMinion(GameObject transferedMinionBlue,GameObject transferedMinionRed,string tag) // 
    {
        if (transferedMinionBlue.transform.GetChild(0).GetComponent<EnemySatatus>()._eminontpye == EMINIONTYPE.Nomal) //ù��° �̴Ͼ����� Ÿ�� ���� �����̸�
        {
            if (tag == "Blue") //�����̰� �±װ� ���� 
            {
                BasicMinion[0] = transferedMinionBlue; //  ��� �����̴Ͼ�
            }
            else // �����̰� �±װ� �����
            {
                BasicMinion[2] = transferedMinionRed; // ���� �����̴Ͼ�

            }
        }

        else // ���Ÿ� �̸鼭
        {
            if (tag == "Blue") // ���Ÿ��鼭 �±װ� ����  
            {
                BasicMinion[1] = transferedMinionBlue; // ��� ���Ÿ� �̴Ͼ�
            }
            else
            {
                BasicMinion[3] = transferedMinionRed; // ���� ���Ÿ� �̴Ͼ�
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
            PhotonNetwork.Instantiate(BasicMinion[0].name, GameManager.Instance.spawnPositions[0].position, Quaternion.identity); // �̰� �ٲ�

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

            PhotonNetwork.Instantiate(BasicMinion[2].name, GameManager.Instance.spawnPositions[1].position, Quaternion.identity); // �̰� �ٲ�

            PhotonNetwork.Instantiate(BasicMinion[3].name, GameManager.Instance.spawnPositions[1].position, Quaternion.identity);
        }
    }
}