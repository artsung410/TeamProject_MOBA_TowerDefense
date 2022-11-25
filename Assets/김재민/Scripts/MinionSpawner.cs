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
        MinionBluePrints[0] = MinionBluePrints[2] = minionDB.itemList[(int)MinionSort.Forest_Bat]; // ����
        MinionBluePrints[1] = MinionBluePrints[3] = minionDB.itemList[(int)MinionSort.Nymph_Fairy]; // ���Ÿ�
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
    // ���                             // ���� 
    void ChangeMinion(int minionID, GameObject transferedMinionBlue,GameObject transferedMinionRed,string tag) // 
    {
        Enemybase blueMinion = transferedMinionBlue.transform.GetChild(0).GetComponent<Enemybase>();

        if (blueMinion._eminontpye == EMINIONTYPE.Nomal) //ù��° �̴Ͼ����� Ÿ�� ���� �����̸�
        {
            if (tag == "Blue") //�����̰� �±װ� ���� 
            {
                MinionBluePrints[0] = minionDB.itemList[minionID - 5001];
                BasicMinion[0] = transferedMinionBlue; //  ��� �����̴Ͼ�
            }
            else // �����̰� �±װ� �����
            {
                MinionBluePrints[2] = minionDB.itemList[minionID - 5001];
                BasicMinion[2] = transferedMinionRed; // ���� �����̴Ͼ�
            }
        }

        else // ���Ÿ� �̸鼭
        {
            if (tag == "Blue") // ���Ÿ��鼭 �±װ� ����  
            {
                MinionBluePrints[1] = minionDB.itemList[minionID - 5001];
                BasicMinion[1] = transferedMinionBlue; // ��� ���Ÿ� �̴Ͼ�
            }
            else
            {
                MinionBluePrints[3] = minionDB.itemList[minionID - 5001];
                BasicMinion[3] = transferedMinionRed; // ���� ���Ÿ� �̴Ͼ�
            }
        }
        SetInitData();
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

    private void SetInitData()
    {
        for(int i = 0; i < BasicMinion.Length; i++)
        {
            SetDetaliData(BasicMinion[i], MinionBluePrints[i]);
        }
    }

    private void SetDetaliData(GameObject minion, MinionBlueprint minionBP)
    {
        minion.transform.GetChild(0).GetComponent<Enemybase>().Damage = MinionBluePrints[0].Attack = minionBP.Attack;
        minion.transform.GetChild(0).GetComponent<Enemybase>().AttackSpeed = MinionBluePrints[0].Attack = minionBP.Attack_Speed;
        minion.transform.GetChild(0).GetComponent<Enemybase>().attackRange = MinionBluePrints[0].Attack = minionBP.Range;
        minion.transform.GetChild(0).GetComponent<Enemybase>().moveSpeed = MinionBluePrints[0].Attack = minionBP.Move_Speed;
        minion.transform.GetChild(0).GetComponent<Enemybase>().HP = MinionBluePrints[0].Attack = minionBP.Hp;
        minion.transform.GetChild(0).GetComponent<Enemybase>().minionSprite = minionBP.Icon;
    }
}