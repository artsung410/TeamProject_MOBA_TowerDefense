using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;

public enum Stat_Columns
{
    HP,             // float
    Dmg,            // float
    Range,          // float
    Atk_Speed,      // float
    Move_Speed,     // float
    Max_Exp,        // int
    Character_ID,   // int
    Exp_Enemy,      // int   
}

public enum HeroType
{
    Warrior,
    Wizard,
}

public class Stats : GoogleSheetManager
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    [Header("ü�� ����")]
    public float MaxHealth = 1;
    //public float health;

    [Header("���� ����")]
    public float attackDmg = 1;
    public float attackSpeed = 1;
    public float attackRange = 1;

    [Header("���� ���")]
    public HeroType AttackType;

    [Header("�̵� ����")]
    public float MoveSpeed = 1;

    [Header("����")]
    public int Level = 1;

    [Header("����ġ")]
    public float Exp;
    public float ExpDetectRange;

    public int maxExp;
    private int minExp;
    private int charID;
    public int enemyExp
    {
        get;
        private set;
    }

    PlayerBehaviour _playerScript;
    Health _health;

    private void Awake()
    {
        _playerScript = GetComponent<PlayerBehaviour>();
        _health = GetComponent<Health>();

        // Ÿ�Կ� ���� �������� ������ �ٸ���
        if (AttackType == HeroType.Warrior)
        {
            StartCoroutine(GetLevelData(warriorURL));
        }
        else if(AttackType == HeroType.Wizard)
        {
            StartCoroutine(GetLevelData(magicionURL));
        }

        // ������ ���
        Health.OnPlayerDieEvent += PlayerLevelUpFactory;
        Enemybase.OnMinionDieEvent += PlayerLevelUpFactory;
        Turret.OnTurretDestroyEvent += PlayerLevelUpFactory;
    }
    public override void SetCharactorDatas(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            levelDatas.Add(new List<string>());
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++)
            {
                levelDatas[i].Add(column[j]);
            }

            CharactorLevelData.Add(i + 1, levelDatas[i]);
        }
    }

    IEnumerator GetLevelData(string url)
    {
        UnityWebRequest GetCharactorData = UnityWebRequest.Get(url);
        yield return GetCharactorData.SendWebRequest();
        SetCharactorDatas(GetCharactorData.downloadHandler.text);

        StatInit();
    }
    public void StatInit()
    {
        MaxHealth = float.Parse(CharactorLevelData[1][(int)Stat_Columns.HP]);
        attackDmg = float.Parse(CharactorLevelData[1][(int)Stat_Columns.Dmg]);
        attackRange = float.Parse(CharactorLevelData[1][(int)Stat_Columns.Range]);
        attackSpeed = float.Parse(CharactorLevelData[1][(int)Stat_Columns.Atk_Speed]);
        MoveSpeed = float.Parse(CharactorLevelData[1][(int)Stat_Columns.Move_Speed]);
        maxExp = int.Parse(CharactorLevelData[1][(int)Stat_Columns.Max_Exp]);
        charID = int.Parse(CharactorLevelData[1][(int)Stat_Columns.Character_ID]);
        enemyExp = int.Parse(CharactorLevelData[1][(int)Stat_Columns.Exp_Enemy]);
    }

    private void OnEnable()
    {
        if (CharactorLevelData.ContainsKey(Level))
        {
            Debug.Log("stat OnEnable Check");
            SetStats(Level);
        }
    }

    private void Start()
    {
        Level = 1;
        MaxHealth = 500;
        attackDmg = 10;
        attackRange = 5;
        attackSpeed = 1;
        MoveSpeed = 15;
        minExp = 0;
        maxExp = 100;
        charID = 1;
        enemyExp = 100;

        ExpDetectRange = 20f;
        //Debug.Log("start�κ� �ʱ�ȭ �Ϸ�");
    }

    // ������ ���� ���� ����
    public void SetStats(int level)
    {
        MaxHealth = float.Parse(CharactorLevelData[level][(int)Stat_Columns.HP]);

        attackDmg = float.Parse(CharactorLevelData[level][(int)Stat_Columns.Dmg]);
        attackRange = float.Parse(CharactorLevelData[level][(int)Stat_Columns.Range]);
        attackSpeed = float.Parse(CharactorLevelData[level][(int)Stat_Columns.Atk_Speed]);

        MoveSpeed = float.Parse(CharactorLevelData[level][(int)Stat_Columns.Move_Speed]);

        maxExp = int.Parse(CharactorLevelData[level][(int)Stat_Columns.Max_Exp]);
        charID = int.Parse(CharactorLevelData[level][(int)Stat_Columns.Character_ID]);
        enemyExp = int.Parse(CharactorLevelData[level][(int)Stat_Columns.Exp_Enemy]);
    }

    public void PlayerLevelUpFactory(GameObject expBag, float exp)
    {
        if ((object)expBag != null)
        {
            // expBag�� ���� tag�� ������ �������̴ϱ� return�Ѵ�
            if (expBag.tag == gameObject.tag)
            {
                return;
            }

            // expBag�� ������ �Ÿ��� ����Ѵ�
            float dist = Vector3.Distance(expBag.transform.position, this.transform.position);

            // TODO : ���� ���� �̺�Ʈ�� �־�� ���� ���� ����
            _playerScript.targetedEnemy = null;

            // �Ÿ��� �νİ����� �Ÿ� ���� �ִٸ� ����ġ ����
            if (dist <= ExpDetectRange)
            {
                Exp += exp;
                // ����ġ�� �ִ� ����ġ���� ������ �������� �Ѵ�
                while (Exp >= maxExp)
                {
                    // 10���� �޼��� �����������ʰ� ����ġ�ٴ� ���� �ִ�ġ �̻����� ���� �ʴ´�
                    if (CharactorLevelData.ContainsKey(Level + 1) == false)
                    {
                        Exp = Mathf.Clamp(Exp, minExp, maxExp);
                        return;
                    }

                    Level++;

                    // Ÿ�� �ر��� ���ӸŴ����� �÷��̾� ������ �޾ƿͼ� �ر��Ѵ�
                    GameManager.Instance.UnlockTower(gameObject.tag, Level);
                    SetStats(Level);
                    photonView.RPC(nameof(_health.LevelHealthUpdate), RpcTarget.All, MaxHealth);

                    // Exp���� maxExp��ŭ ���� �������� �����ϱ�
                    Exp = Mathf.Max(Exp - maxExp, 0);
                }
            }
        }

    }

    private void Update()
    {
        if (gameObject.tag == "Blue")
        {
            GameManager.Instance.PlayerLevel1 = Level;
        }
        else
        {
            GameManager.Instance.PlayerLevel2 = Level;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, attackRange);
    }


}