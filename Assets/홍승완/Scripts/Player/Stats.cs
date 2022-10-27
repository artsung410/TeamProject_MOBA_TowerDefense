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
        attackSpeed = 1f;

        MoveSpeed = 15f;
    }
}


