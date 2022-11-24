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

    [Header("체력 스탯")]
    public float MaxHealth = 1;
    //public float health;

    [Header("공격 스탯")]
    public float attackDmg = 1;
    public float attackSpeed = 1;
    public float attackRange = 1;

    [Header("공격 방식")]
    public HeroType AttackType;

    // TODO : 이동속도 버프, 디버프 관련해서 새로운 변수 추가할 필요있음
    [Header("이동 관련")]
    public float MoveSpeed = 1;

    [Header("레벨")]
    public int Level;

    [Header("경험치")]
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

        // 타입에 따라 가져오는 스탯이 다르다
        if (AttackType == HeroType.Warrior)
        {
            StartCoroutine(GetLevelData(warriorURL));
        }
        else if(AttackType == HeroType.Wizard)
        {
            StartCoroutine(GetLevelData(magicionURL));
        }

        // 구독자 등록
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
        //Debug.Log("플레이어 스탯 초기화 ####");
    }
    public void StatInit()
    {
        //Level = 1;

        MaxHealth = float.Parse(CharactorLevelData[Level][(int)Stat_Columns.HP]);
        attackDmg = float.Parse(CharactorLevelData[Level][(int)Stat_Columns.Dmg]);
        attackRange = float.Parse(CharactorLevelData[Level][(int)Stat_Columns.Range]);
        attackSpeed = float.Parse(CharactorLevelData[Level][(int)Stat_Columns.Atk_Speed]);
        MoveSpeed = float.Parse(CharactorLevelData[Level][(int)Stat_Columns.Move_Speed]);
        maxExp = int.Parse(CharactorLevelData[Level][(int)Stat_Columns.Max_Exp]);
        charID = int.Parse(CharactorLevelData[Level][(int)Stat_Columns.Character_ID]);
        enemyExp = int.Parse(CharactorLevelData[Level][(int)Stat_Columns.Exp_Enemy]);

        //Debug.Log("코루틴 부분 초기화 완료");
        // 실험결과 코루틴 부분이 start보다 나중에 완료 되었다
    }



    private void OnEnable()
    {
        
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
        //Debug.Log("start부분 초기화 완료");
    }

    private void Update()
    {
        
    }

    // 레벨에 따른 스텟 증가
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

    //[PunRPC]
    public void PlayerLevelUpFactory(GameObject expBag, float exp)
    {
        if (expBag == null)
        {
            return;
        }

        // expBag와 나의 tag가 같으면 같은팀이니까 return한다
        if (expBag.tag == gameObject.tag)
        {
            return;
        }

        // expBag과 나와의 거리를 계산한다
        float dist = Vector3.Distance(expBag.transform.position, this.transform.position);

        // TODO : 상대방 죽음 이벤트에 넣어둠 추후 개선 사항
        _playerScript.targetedEnemy = null;

        // 거리가 인식가능한 거리 내에 있다면 경험치 얻음
        if (dist <= ExpDetectRange)
        {
            Exp += exp;
            // 경험치가 최대 경험치보다 높으면 레벨업을 한다
            while (Exp >= maxExp)
            {
                // 10레벨 달성시 레벨업하지않고 경험치바는 차되 최대치 이상으론 차지 않는다
                if (CharactorLevelData.ContainsKey(Level + 1) == false)
                {
                    Exp = Mathf.Clamp(Exp, minExp, maxExp);
                    return;
                }

                Level++;

                // 타워 해금은 게임매니저가 플레이어 레벨을 받아와서 해금한다
                GameManager.Instance.UnlockTower(gameObject.tag, Level);
                SetStats(Level);
                photonView.RPC(nameof(_health.HealthUpdate), RpcTarget.All, MaxHealth);

                // Exp에서 maxExp만큼 뺀다 레벨업을 했으니까
                Exp = Mathf.Max(Exp - maxExp, 0);
            }
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, attackRange);
    }


    private void OnDisable()
    {
        //StopAllCoroutines();
    }


}