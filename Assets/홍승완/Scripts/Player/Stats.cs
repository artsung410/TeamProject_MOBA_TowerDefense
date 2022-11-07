using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;

public enum Stat_Columns
{
    HP,
    Dmg,
    Range,
    Atk_Speed,
    Move_Speed,
    Min_Exp,
    Max_Exp,
}

public class Stats : GoogleSheetManager
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################
    [Header("ü�� ����")]
    public float MaxHealth;
    //public float health;

    [Header("���� ����")]
    public float attackDmg;
    public float attackSpeed;
    public float attackRange;
    
    // TODO : �̵��ӵ� ����, ����� �����ؼ� ���ο� ���� �߰��� �ʿ�����
    [Header("�̵� ����")]
    public float MoveSpeed;

    [Header("����")]
    public int Level;
    int currentLevel;
    int deltaLevel;

    private float currentExp;
    private float minExp;
    private float maxExp;

    PlayerBehaviour _playerScript;

    // dictinary�� ����� stat���� -> key���� ���� ����

    private void Awake()
    {
        _playerScript = GetComponent<PlayerBehaviour>();
    }

    private void OnEnable()
    {
        StartCoroutine(GetLevelData());
    }
    IEnumerator GetLevelData()
    {
        UnityWebRequest GetWarriorData = UnityWebRequest.Get(WarriorURL);
        yield return GetWarriorData.SendWebRequest();
        SetWarriorStats(GetWarriorData.downloadHandler.text);
        StatInit();
    }

    private void Start()
    {
    }

    public void StatInit()
    {
        Level = 1;
        currentLevel = Level;

        MaxHealth = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.HP]);

        attackDmg = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Dmg]);
        attackRange = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Range]);
        attackSpeed = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Atk_Speed]);

        MoveSpeed = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Move_Speed]);

        currentExp = 0f;
        minExp = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Min_Exp]);
        maxExp = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Max_Exp]);
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        deltaLevel++;

        Level = currentLevel + deltaLevel;
        IncreaseStat(Level);
        currentLevel = Level;
        deltaLevel = 0;
    }

    public void IncreaseStat(int level)
    {
        MaxHealth = float.Parse(WarriorLevelData[level][(int)Stat_Columns.HP]);

        attackDmg = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Dmg]);
        attackRange = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Range]);
        attackSpeed = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Atk_Speed]);

        MoveSpeed = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Move_Speed]);

        minExp = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Min_Exp]);
        maxExp = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Max_Exp]);
    }
}