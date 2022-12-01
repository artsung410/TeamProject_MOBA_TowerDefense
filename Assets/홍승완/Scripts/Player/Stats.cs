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
    public float maxHealth = 1;
    //public float health;

    [Header("���� ����")]
    public float attackDmg = 1;
    public float attackSpeed = 1;
    public float attackRange = 1;

    [Header("���� ���")]
    public HeroType AttackType;

    [Header("�̵� ����")]
    public float moveSpeed = 1;

    [Header("����")]
    public int level = 1;

    [Header("����ġ")]
    public float acquiredExp;
    public float expDetectRange;
    public int maxExp;
    public int enemyExp;

    [Header("��������")]
    public float buffMaxHealth;
    public float buffAttackDamge;
    public float buffAttackSpeed;
    public float buffAttackRange;
    public float buffMoveSpeed;

    #region ParsingData
    private float _parseMaxHealth;
    private float _parseAttackDmg;
    private float _parseAttackSpeed;
    private float _parseAttackRange;
    private float _parseMoveSpeed;
    private int _parseMinExp;
    private int _parseMaxExp;
    private int _parseCharID;
    private int _parseEnemyExp;
    #endregion


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

        StatDataParse(1);
        SetPlayerStats();
    }

    private void StatDataParse(int level)
    {
        _parseMaxHealth = float.Parse(CharactorLevelData[level][(int)Stat_Columns.HP]);
        _parseAttackDmg = float.Parse(CharactorLevelData[level][(int)Stat_Columns.Dmg]);
        _parseAttackRange = float.Parse(CharactorLevelData[level][(int)Stat_Columns.Range]);
        _parseAttackSpeed = float.Parse(CharactorLevelData[level][(int)Stat_Columns.Atk_Speed]);
        _parseMoveSpeed = float.Parse(CharactorLevelData[level][(int)Stat_Columns.Move_Speed]);
        _parseMaxExp = int.Parse(CharactorLevelData[level][(int)Stat_Columns.Max_Exp]);
        _parseCharID = int.Parse(CharactorLevelData[level][(int)Stat_Columns.Character_ID]);
        _parseEnemyExp = int.Parse(CharactorLevelData[level][(int)Stat_Columns.Exp_Enemy]);
    }

    public void SetPlayerStats()
    {
        maxHealth = _parseMaxHealth + buffMaxHealth;
        attackDmg = _parseAttackDmg + buffAttackDamge;
        attackRange = _parseAttackRange + buffAttackRange;
        attackSpeed = _parseAttackSpeed + buffAttackSpeed;
        moveSpeed = _parseMoveSpeed + buffMoveSpeed;
        maxExp = _parseMaxExp;
        enemyExp = _parseEnemyExp;
    }

    private void OnEnable()
    {
        if (CharactorLevelData.ContainsKey(level))
        {
            Debug.Log("stat OnEnable Check");
            StatDataParse(level);
            SetPlayerStats();
        }
    }

    private void Start()
    {
        level = 1;
        maxHealth = 500;
        attackDmg = 10;
        attackRange = 5;
        attackSpeed = 1;
        moveSpeed = 15;
        _parseMinExp = 0;
        maxExp = 100;
        _parseCharID = 1;
        enemyExp = 100;

        expDetectRange = 20f;

        SetPlayerStats();
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
            if (dist <= expDetectRange)
            {
                this.acquiredExp += exp;
                // ����ġ�� �ִ� ����ġ���� ������ �������� �Ѵ�
                while (this.acquiredExp >= maxExp)
                {
                    // 10���� �޼��� �����������ʰ� ����ġ�ٴ� ���� �ִ�ġ �̻����� ���� �ʴ´�
                    if (CharactorLevelData.ContainsKey(level + 1) == false)
                    {
                        this.acquiredExp = Mathf.Clamp(this.acquiredExp, _parseMinExp, maxExp);
                        return;
                    }

                    level++;

                    // Ÿ�� �ر��� ���ӸŴ����� �÷��̾� ������ �޾ƿͼ� �ر��Ѵ�
                    GameManager.Instance.UnlockTower(gameObject.tag, level);
                    StatDataParse(level);
                    SetPlayerStats();
                    photonView.RPC(nameof(_health.LevelHealthUpdate), RpcTarget.All, maxHealth);

                    // Exp���� maxExp��ŭ ���� �������� �����ϱ�
                    this.acquiredExp = Mathf.Max(this.acquiredExp - maxExp, 0);
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, attackRange);
    }

}