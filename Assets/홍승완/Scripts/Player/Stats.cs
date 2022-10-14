using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################
    [Header("ü�� ����")]
    public float maxHealth;
    public float health;

    [Header("���� ����")]
    public float attackDmg;
    public float attackSpeed;
    public float attackTime;
    public float attackRange;

    //HeroCombat _heroCombatScripts;
    PlayerBehaviour _playerScript;

    private void Awake()
    {
        //_heroCombatScripts = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCombat>();

        _playerScript = GetComponent<PlayerBehaviour>();
    }

    void Start()
    {
        StatInit();
    }

    private void Update()
    {
        DieAndDestroy();
    }


    private void DieAndDestroy()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            _playerScript.targetedEnemy = null;
            _playerScript.perfomMeleeAttack = false;
        }
    }

    private void StatInit()
    {
        //health = maxHealth;
        //maxHealth = 100f;
        //attackDmg = 5f;
        //attackSpeed = 1f;
        //attackTime = 1.4f;
        //if (gameObject.CompareTag("Player"))
        //{
        //    maxHealth = 100f;
        //    health = maxHealth;
        //    attackDmg = 5f;
        //    attackSpeed = 1f;
        //    attackTime = 1.4f;
        //}

        //if (gameObject.CompareTag("Enemy"))
        //{
        //    maxHealth = 50f;
        //    health = maxHealth;

        //}
    }
}
