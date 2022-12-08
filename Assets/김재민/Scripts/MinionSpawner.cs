using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum MinionSort
{
    Forest_Bat = 15,
    Nymph_Fairy = 40,
}

public class MinionSpawner : MonoBehaviourPunCallbacks
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    [Header("�⺻�̴Ͼ�")]
    public GameObject[] BasicMinion;

    [Header("�̴Ͼ� ������ ����")]
    public MinionBlueprint[] MinionBluePrints;

    [Header("�ٲ� �̴Ͼ� Id ����")]
    public int[] MinionIDs;

    [HideInInspector]
    public int BlueSkillWave;
    [HideInInspector]
    public int RedSkillWave;

    [HideInInspector]
    public int BlueSkillSpawnSize;
    [HideInInspector]
    public int RedSkillSpawnSize;


    [HideInInspector]
    public bool skillOn;

    [HideInInspector]
    public string tag;

    //��ų�̴Ͼ� ����
    float elaspedTime;
    float minionSpawnTime = 20f;
    
    //=================================================
    public static MinionSpawner Instance;
    public MinionDatabaseList minionDB_List;


    private void Awake()
    {
        Instance = this;
        MinionBluePrints[0] = MinionBluePrints[2] = minionDB_List.itemList[(int)MinionSort.Forest_Bat]; // ����
        MinionBluePrints[1] = MinionBluePrints[3] = minionDB_List.itemList[(int)MinionSort.Nymph_Fairy]; // ���Ÿ�

        MinionIDs[0] = MinionIDs[2] = minionDB_List.itemList[(int)MinionSort.Forest_Bat].ID; // ����
        MinionIDs[1] = MinionIDs[3] = minionDB_List.itemList[(int)MinionSort.Nymph_Fairy].ID; // ���Ÿ�
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
        elaspedTime += Time.deltaTime;
        if (elaspedTime >= minionSpawnTime)
        {
            elaspedTime = 0;
            BlueSpawnMinion();
            RedSpawnMinion();
            if (tag == "Blue" && BlueSkillWave > 0)
            {
                for (int i = 0; i < BlueSkillSpawnSize; i++)
                {
                    BlueSpawnMinion();
                }
                BlueSkillWave--;
            }
            else if (tag == "Red" && RedSkillWave > 0)
            {
                for (int i = 0; i < BlueSkillSpawnSize; i++)
                {
                    RedSpawnMinion();
                }
                RedSkillWave--;
            }
        }
    }
    // ���                             // ���� 
    void ChangeMinion(int minionID, GameObject transferedMinionBlue, GameObject transferedMinionRed, string tag) // 
    {
        Enemybase blueMinion = transferedMinionBlue.transform.GetChild(0).GetComponent<Enemybase>();

        if (blueMinion._eminontpye == EMINIONTYPE.Nomal) //ù��° �̴Ͼ����� Ÿ�� ���� �����̸�
        {
            if (tag == "Blue") //�����̰� �±װ� ���� 
            {
                MinionBluePrints[0] = minionDB_List.itemList[minionID - 5001];
                BasicMinion[0] = transferedMinionBlue; //  ��� �����̴Ͼ�
                MinionIDs[0] = minionID;
            }
            else // �����̰� �±װ� �����
            {
                MinionBluePrints[2] = minionDB_List.itemList[minionID - 5001];
                BasicMinion[2] = transferedMinionRed; // ���� �����̴Ͼ�
                MinionIDs[2] = minionID;
            }
        }

        else // ���Ÿ� �̸鼭
        {
            if (tag == "Blue") // ���Ÿ��鼭 �±װ� ����  
            {
                MinionBluePrints[1] = minionDB_List.itemList[minionID - 5001];
                BasicMinion[1] = transferedMinionBlue; // ��� ���Ÿ� �̴Ͼ�
                MinionIDs[1] = minionID;
            }
            else
            {
                MinionBluePrints[3] = minionDB_List.itemList[minionID - 5001];
                BasicMinion[3] = transferedMinionRed; // ���� ���Ÿ� �̴Ͼ�
                MinionIDs[3] = minionID;
            }
        }
    }

    private GameObject newMinion1;
    private GameObject newMinion2;
    private GameObject newMinion3;
    private GameObject newMinion4;

    private void BlueSpawnMinion()
    {

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            if (BasicMinion[0] == null || BasicMinion[1] == null)
            {
                return;
            }

            newMinion1 = PhotonNetwork.Instantiate(BasicMinion[0].name, GameManager.Instance.SpawnMinionPosition[0].position, Quaternion.identity); // �̰� �ٲ�
            newMinion1.transform.GetChild(0).GetComponent<Enemybase>().SetInitData(MinionIDs[0]);

            newMinion2 = PhotonNetwork.Instantiate(BasicMinion[1].name, GameManager.Instance.SpawnMinionPosition[0].position, Quaternion.identity);
            newMinion2.transform.GetChild(0).GetComponent<Enemybase>().SetInitData(MinionIDs[1]);
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

            newMinion3 = PhotonNetwork.Instantiate(BasicMinion[2].name, GameManager.Instance.SpawnMinionPosition[1].position, Quaternion.identity); // �̰� �ٲ�
            newMinion3.transform.GetChild(0).GetComponent<Enemybase>().SetInitData(MinionIDs[2]);

            newMinion4 = PhotonNetwork.Instantiate(BasicMinion[3].name, GameManager.Instance.SpawnMinionPosition[1].position, Quaternion.identity);
            newMinion4.transform.GetChild(0).GetComponent<Enemybase>().SetInitData(MinionIDs[3]);
        }
    }
}