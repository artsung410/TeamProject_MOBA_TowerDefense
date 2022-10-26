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
    // ###############################################

    NavMeshAgent agent;
    Animator animator;
    Stats playerStats;
    PlayerBehaviour _playerScript;
    Health hp;

    float motionSmoothTime = 0.1f;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<Stats>();
        _playerScript = GetComponent<PlayerBehaviour>();
        hp = GetComponent<Health>();
    }

    private void OnEnable()
    {
        //AliveMotion();
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
            animator.SetTrigger("Die");
        }
    }

    private void AliveMotion()
    {
        if (hp.isDeath == false)
        {
            animator.SetTrigger("Alive");
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
        //Debug.Log("check");
    }

    //float attackAnimationPose = 0;
    private void CombatMotion()
    {
        //Debug.Log(randomPose);
        if (_playerScript.targetedEnemy != null)
        {
            if (_playerScript.perfomMeleeAttack == true)
            {
                // 공격 모션 재생
                animator.SetBool("Attack", true);
                if (photonView.IsMine)
                {
                    _playerScript.IsAttack = true;  

                }
                // 무작위 공격 자세
                //animator.SetFloat("AttackPose", attackAnimationPose);
                // 공격 모션 재생 속도
                animator.SetFloat("AttackSpeed", playerStats.attackSpeed);
                StartCoroutine(MeleeAttackInterval());
            }
        }
        else
        {
            if (_playerScript.perfomMeleeAttack == false)
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
