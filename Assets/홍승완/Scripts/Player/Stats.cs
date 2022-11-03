using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Stats : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################
    [Header("체력 스탯")]
    public float StartHealth;
    //public float health;

    [Header("공격 스탯")]
    public float attackDmg;
    public float attackSpeed;
    public float attackTime;
    public float attackRange;
    
    // TODO : 이동속도 버프, 디버프 관련해서 새로운 변수 추가할 필요있음
    [Header("이동 관련")]
    public float MoveSpeed;

    //HeroCombat _heroCombatScripts;
    PlayerBehaviour _playerScript;

    private void Awake()
    {
        _playerScript = GetComponent<PlayerBehaviour>();
    }

    private void OnEnable()
    {
        StatInit();
    }

    private void StatInit()
    {
        StartHealth = 250f;

        attackDmg = 35f;
        attackRange = 6f;
        attackSpeed = 1.6f;

        MoveSpeed = 15f;
    }
}


