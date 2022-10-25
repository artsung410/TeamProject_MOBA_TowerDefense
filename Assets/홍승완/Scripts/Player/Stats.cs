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
        //_heroCombatScripts = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCombat>();

        _playerScript = GetComponent<PlayerBehaviour>();
    }

    private void OnEnable()
    {
        StatInit();
        
    }

    void Start()
    {
    }

    private void Update()
    {
        //DieAndDestroy();
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    MoveSpeed = 3f;
        //}
    }



    //private void DieAndDestroy()
    //{
    //    if (health <= 0)
    //    {
    //        Destroy(gameObject);
    //        _playerScript.targetedEnemy = null;
    //        _playerScript.perfomMeleeAttack = false;
    //    }
    //}

    private void StatInit()
    {
        MoveSpeed = 10f;
        StartHealth = 2000f;
        attackDmg = 2f;
        attackRange = 3f;
        
    }
}
