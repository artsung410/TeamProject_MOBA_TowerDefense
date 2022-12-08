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

public class Stats : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################


    [Header("체력 스탯")]
    public float maxHealth;
    //public float health;

    [Header("공격 스탯")]
    public float attackDmg;
    public float attackSpeed;
    public float attackRange;

    [Header("공격 방식")]
    public HeroType AttackType;

    [Header("이동 관련")]
    public float moveSpeed;

    [Header("레벨")]
    public int Level;

    [Header("경험치")]
    public float acquiredExp;
    public float expDetectRange;
    public int maxExp;
    public int enemyExp;

    [Header("버프변수")]
    public float buffMaxHealth = 0f;
    public float buffAttackDamge = 0f;
    public float buffAttackSpeed = 0f;
    public float buffAttackRange = 0f;
    public float buffMoveSpeed = 0f;

    private PlayerStatDatas myStat;
    private PlayerBehaviour _playerScript;
    private Health _health;

    private void Awake()
    {
        _playerScript = GetComponent<PlayerBehaviour>();
        _health = GetComponent<Health>();
        Level = 1;
        // 구독자 등록
        Health.OnPlayerDieEvent += PlayerLevelUpFactory;
        Enemybase.OnMinionDieEvent += PlayerLevelUpFactory;
        Turret.OnTurretDestroyEvent += PlayerLevelUpFactory;
    }

    private void OnEnable()
    {
        //Debug.Log($"is work? : { CSVtest.Instance.wizardStatParsing.dataList[0].hp}");
        SetPlayerStats(AttackType, Level);
    }


    public void SetPlayerStats(HeroType type, int level)
    {
        if (type == HeroType.Warrior)
        {
            myStat = CSVtest.Instance.warriorStatParsing.dataList[level - 1];
        }
        else if (type == HeroType.Wizard)
        {
            myStat = CSVtest.Instance.wizardStatParsing.dataList[level - 1];
        }

        maxHealth = myStat.hp + buffMaxHealth;
        attackDmg = myStat.damage + buffAttackDamge;
        attackRange = myStat.range + buffAttackRange;
        attackSpeed = myStat.attackSpeed + buffAttackSpeed;
        moveSpeed = myStat.moveSpeed + buffMoveSpeed;
        maxExp = myStat.maxExp;
        enemyExp = myStat.expEnemy;
        expDetectRange = 25f;
    }


    public void SetBuff()
    {
        maxHealth = myStat.hp + buffMaxHealth;
        attackDmg = myStat.damage + buffAttackDamge;
        attackRange = myStat.range + buffAttackRange;
        attackSpeed = myStat.attackSpeed + buffAttackSpeed;
        moveSpeed = myStat.moveSpeed + buffMoveSpeed;
    }

    public void PlayerLevelUpFactory(GameObject expBag, float exp)
    {
        if ((object)expBag != null)
        {
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
            if (dist <= expDetectRange)
            {
                this.acquiredExp += exp;
                // 경험치가 최대 경험치보다 높으면 레벨업을 한다
                while (this.acquiredExp >= maxExp)
                {
                    // maxLevel 달성시 레벨업하지않고 경험치바는 차되 최대치 이상으론 차지 않는다
                    if (myStat.charID == 0)
                    {
                        this.acquiredExp = Mathf.Clamp(this.acquiredExp, 0, maxExp);
                        return;
                    }
                    Level++;
                    SetPlayerStats(AttackType, Level);
                    // 타워 해금은 게임매니저가 플레이어 레벨을 받아와서 해금한다
                    GameManager.Instance.UnlockTower(gameObject.tag, Level);
                    photonView.RPC(nameof(_health.LevelHealthUpdate), RpcTarget.All, maxHealth);

                    // Exp에서 maxExp만큼 뺀다 레벨업을 했으니까
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

    private void OnDestroy()
    {
        Enemybase.OnMinionDieEvent -= PlayerLevelUpFactory;
    }

}