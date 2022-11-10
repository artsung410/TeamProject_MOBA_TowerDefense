using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System;


public class PlayerAnimation : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com
    //             MAIL : minsub4400@gmail.com
    // ###############################################

    // TODO : 공격방식에따라 재생되는 모션을 다르게할것

    public static PlayerAnimation instance;
    public Animator animator;

    NavMeshAgent agent;
    Stats playerStats;
    PlayerBehaviour _playerScript;
    Health hp;

    float motionSmoothTime = 0.1f;
    
    void Awake()
    {
        instance = this;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<Stats>();
        _playerScript = GetComponent<PlayerBehaviour>();
        hp = GetComponent<Health>();
    }

    private void OnEnable()
    {
        AliveMotion();
    }


    void Update()
    {
        MoveAniMotion();
        CombatMotion();
    }

    public void DieMotion()
    {
        if (hp.isDeath)
        {
            animator.SetBool("Die", true);
        }
    }

    private void AliveMotion()
    {
        if (hp.isDeath == false)
        {
            animator.SetBool("Die", false);
        }
    }

    private void MoveAniMotion()
    {
        float speed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("Speed", speed, motionSmoothTime, Time.deltaTime);
    }

    IEnumerator MeleeAttackInterval()
    {
        yield return new WaitForSeconds(2f);
        _playerScript.perfomMeleeAttack = false;
    }

    IEnumerator RangeAttackInterval()
    {
        yield return new WaitForSeconds(2f);
        _playerScript.perfomRangeAttack = false;
    }

    private void CombatMotion()
    {
        if (_playerScript.targetedEnemy != null)
        {
            if (_playerScript.perfomMeleeAttack == true || _playerScript.perfomRangeAttack == true)
            {
                if (photonView.IsMine)
                {
                    _playerScript.IsAttack = true;  

                }
                // 공격 모션 재생
                animator.SetBool("Attack", true);

                // 공격 모션 재생 속도
                animator.SetFloat("AttackSpeed", playerStats.attackSpeed);
                StartCoroutine(MeleeAttackInterval());
            }
        }
        else
        {
            if (_playerScript.perfomMeleeAttack == false || _playerScript.perfomRangeAttack == true)
            {
                //Debug.Log("공격 취소");
                if (photonView.IsMine)
                {
                    _playerScript.IsAttack = false;

                }
                animator.SetBool("Attack", false);
                StopCoroutine(MeleeAttackInterval());
            }
        }
    }

    
}
