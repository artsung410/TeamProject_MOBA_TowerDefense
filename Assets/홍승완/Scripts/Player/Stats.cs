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
    public float MaxHealth = 1;
    //public float health;

    [Header("공격 스탯")]
    public float attackDmg = 1;
    public float attackSpeed = 1;
    public float attackRange = 1;

    // TODO : 이동속도 버프, 디버프 관련해서 새로운 변수 추가할 필요있음
    [Header("이동 관련")]
    public float MoveSpeed = 1;

    public int Level;

    public float Exp;

    private float minExp;
    private float maxExp;

    PlayerBehaviour _playerScript;
    Health _health;

    // dictinary를 사용한 stat설정 -> key값은 현재 레벨

    private void Awake()
    {
        _playerScript = GetComponent<PlayerBehaviour>();
        _health = GetComponent<Health>();

        StartCoroutine(GetLevelData());
    }

    IEnumerator GetLevelData()
    {
        UnityWebRequest GetWarriorData = UnityWebRequest.Get(WarriorURL);
        yield return GetWarriorData.SendWebRequest();
        SetWarriorStats(GetWarriorData.downloadHandler.text);

        // 초기화가 너무 느림 => 처음 변수 초기화는 직접 값을 써야할까?
        StatInit();
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
    }

    public void StatInit()
    {
        Level = 1;

        MaxHealth = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.HP]);
        attackDmg = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Dmg]);
        attackRange = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Range]);
        attackSpeed = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Atk_Speed]);
        MoveSpeed = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Move_Speed]);
        minExp = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Min_Exp]);
        maxExp = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Max_Exp]);
    }



    private void Update()
    {

        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                int exp = 30;
                Debug.Log($"얻은 경험치 : {exp}");
                PlayerLevelUp(exp);
                //photonView.RPC(nameof(PlayerLevelUp), RpcTarget.All, exp);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                int exp = 3000;
                Debug.Log($"얻은 경험치 : {exp}");
                PlayerLevelUp(exp);
                //photonView.RPC(nameof(PlayerLevelUp), RpcTarget.All, exp);
            }
        }

        //if (Level == 1 || Level == 3 || Level == 5 || Level == 7)
        //{
        //    // 타워 하나씩 생성
        //    GameManager.Instance.UnlockTower();
        //}
    }


    // 레벨에 따른 스텟 증가
    public void SetStats(int level)
    {
        MaxHealth = float.Parse(WarriorLevelData[level][(int)Stat_Columns.HP]);

        attackDmg = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Dmg]);
        attackRange = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Range]);
        attackSpeed = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Atk_Speed]);

        MoveSpeed = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Move_Speed]);

        minExp = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Min_Exp]);
        maxExp = float.Parse(WarriorLevelData[level][(int)Stat_Columns.Max_Exp]);
    }

    [PunRPC]
    public void PlayerLevelUp(float exp)
    {
        Exp += exp;

        // 경험치가 최대 경험치보다 높으면 레벨업을 한다
        while (Exp >= maxExp)
        {
            // 10레벨 달성시 레벨업하지않고 경험치바는 차되 최대치 이상으론 차지 않는다
            if (WarriorLevelData.ContainsKey(Level + 1) == false)
            {
                Exp = Mathf.Clamp(Exp, minExp, maxExp);
                return;
            }
            Level++;
            GameManager.Instance.UnlockTower(Level);
            SetStats(Level);
            photonView.RPC(nameof(_health.HealthUpdate), RpcTarget.All, MaxHealth);
            // Exp에서 maxExp만큼 뺀다 레벨업을 했으니까
            Exp = Mathf.Max(Exp - maxExp, 0);
        }

    }

    // TODO : 경험치 관리는 어떤식으로?
    
    // 일정범위내(overlapsphere)의 적이 사망(die호출시)할시 플레이어에게 경험치를준다(단, 같은팀 예외(tag처리할것))

    // TODO : 1/3/5/7레벨에 타워가 하나씩 해금(?)된다

}