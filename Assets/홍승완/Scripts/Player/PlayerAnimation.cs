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

    Vector3 AfterPos;
    Vector3 velo = Vector3.forward;
    public Vector3 TargetPos;


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

    private void CombatMotion()
    {
        //Debug.Log(randomPose);
        if (_playerScript.targetedEnemy != null)
        {
            if (_playerScript.perfomMeleeAttack == true)
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
