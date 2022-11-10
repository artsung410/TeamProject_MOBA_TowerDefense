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

    [Header("레벨")]
    public int Level;

    [Header("경험치")]
    public float Exp;
    public float ExpDetectRange;

    private float minExp;
    private float maxExp;

    PlayerBehaviour _playerScript;
    Health _health;

    private void Awake()
    {
        _playerScript = GetComponent<PlayerBehaviour>();
        _health = GetComponent<Health>();
        Debug.Log("Awake 켜짐");
        StartCoroutine(GetLevelData());

        // 구독자 등록
        Health.OnPlayerDieEvent += PlayerLevelUpFactory;
        Enemybase.OnMinionDieEvent += PlayerLevelUpFactory;
        Turret.OnTurretDestroyEvent += PlayerLevelUpFactory;
    }

    IEnumerator GetLevelData()
    {
        UnityWebRequest GetWarriorData = UnityWebRequest.Get(WarriorURL);
        yield return GetWarriorData.SendWebRequest();
        SetWarriorStats(GetWarriorData.downloadHandler.text);

        // 초기화가 너무 느림 => 처음 변수 초기화는 직접 값을 써야할까?
        StatInit();
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

        ExpDetectRange = 20f;
    }

    public void StatInit()
    {
        Level = 1;

        MaxHealth = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.HP]);
        Debug.Log($"파싱한 체력 수치 : {MaxHealth}");
        attackDmg = float.Parse(WarriorLevelData[Level][(int)Stat_Columns.Dmg]) + 100f;
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
            //if (Input.GetKeyDown(KeyCode.I))
            //{
            //    int exp = 30;
            //    Debug.Log($"얻은 경험치 : {exp}");
            //    PlayerLevelUpFactory(exp);
            //    //photonView.RPC(nameof(PlayerLevelUp), RpcTarget.All, exp);
            //}
            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    int exp = 3000;
            //    Debug.Log($"얻은 경험치 : {exp}");
            //    PlayerLevelUpFactory(exp);
            //    //photonView.RPC(nameof(PlayerLevelUp), RpcTarget.All, exp);
            //}
        }
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
    public void PlayerLevelUpFactory(GameObject expBag, float exp)
    {
        // expBag와 나의 tag가 같으면 같은팀이니까 return한다
        if (expBag.tag == gameObject.tag)
        {
            return;
        }

        // expBag과 나와의 거리를 계산한다
        float dist = Vector3.Distance(expBag.transform.position, this.transform.position);
        Debug.Log($"죽은 {expBag.name}과 나와의 거리 : {dist}");
        
        // 거리가 인식가능한 거리 내에 있다면 경험치 얻음
        if (dist <= ExpDetectRange)
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

                // 타워 해금은 게임매니저가 플레이어 레벨을 받아와서 해금한다
                GameManager.Instance.UnlockTower(Level);
                SetStats(Level);
                photonView.RPC(nameof(_health.HealthUpdate), RpcTarget.All, MaxHealth);

                // Exp에서 maxExp만큼 뺀다 레벨업을 했으니까
                Exp = Mathf.Max(Exp - maxExp, 0);
            }
        }


    }

    // TODO : 일정범위내(overlapsphere)의 적이 사망(die호출시)할시 플레이어에게 경험치를준다(단, 같은팀 예외(tag처리할것))

    public float tempExp;
    float rangeMinionExp = 30f;
    float meleeMinionExp = 40f;

    // 기각 이유 : 죽은 적 판별할때 연속해서 값이 들어옴
    //IEnumerator ExpDetector()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        Collider[] expBag = Physics.OverlapSphere(transform.position, ExpDetectRange);
    //        //Debug.Log($"주변 콜라이더들 : {expBag.Length}");
    //        if (expBag.Length > 0)
    //        {
    //            foreach (Collider _exps in expBag)
    //            {
    //                // 검출된 콜라이더들중 죽은객체의 정보 받아오기
    //                // 죽은 객체가 [플레이어]면 {100}
    //                // 죽은 객체가 [근거리 미니언]이면 {40}
    //                // 죽은 객체가 [원거리 미니언]이면 {30}
    //                // 죽은 객체가 [특수 미니언]이면 {50}
    //                // 죽은 객체가 [타워]면 {200}
    //                //Debug.Log($"주변 콜라이더 : {_exps.gameObject.name}");

    //                // 콜라이더중 적만 검색
    //                if (_exps.gameObject.tag == _playerScript.EnemyTag)
    //                {
    //                    // 적 플레이어
    //                    if (_exps.gameObject.GetComponent<Health>() != null)
    //                    {
    //                        Debug.Log("적 확인");
    //                        Health enemyPlayer = _exps.gameObject.GetComponent<Health>();
    //                        // 적이 죽었으면
    //                        if (enemyPlayer.isDeath == true)
    //                        {
    //                            // 경험치를 얻는다
    //                            tempExp += 30f;
    //                        }
    //                    }

    //                    if (_exps.gameObject.GetComponent<Enemybase>() != null)
    //                    {
    //                        Debug.Log("적 미니언 확인");
    //                        Enemybase enemyMinion = _exps.gameObject.GetComponent<Enemybase>();

    //                        if (enemyMinion.isDead == true)
    //                        {
    //                            tempExp += 15f;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, ExpDetectRange);
    }


    private void OnDisable()
    {
        //Debug.Log("Stats disable호출");
        StopAllCoroutines();
    }
}