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
    [Header("체력 스탯")]
    public float MaxHealth;
    //public float health;

    [Header("공격 스탯")]
    public float attackDmg;
    public float attackSpeed;
    public float attackRange;
    
    // TODO : 이동속도 버프, 디버프 관련해서 새로운 변수 추가할 필요있음
    [Header("이동 관련")]
    public float MoveSpeed;

    [Header("레벨")]
    public int Level;
    int currentLevel;
    int deltaLevel;

    private float currentExp;
    private float minExp;
    private float maxExp;

    PlayerBehaviour _playerScript;

    // dictinary를 사용한 stat설정 -> key값은 현재 레벨

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