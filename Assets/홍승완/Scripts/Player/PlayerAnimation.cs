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

    // TODO : 공격방식에따라 재생되는 모션을 다르게할것

    public static PlayerAnimation instance;
    public Animator animator;
    public GameObject Chractor;

    NavMeshAgent agent;
    Stats playerStats;
    PlayerBehaviour _playerScript;
    Health hp;
    HeroAbility _abillity;

    float motionSmoothTime = 0.1f;

    public bool IsAttack;

    void Awake()
    {
        instance = this;

        agent = Chractor.GetComponent<NavMeshAgent>();
        playerStats = Chractor.GetComponent<Stats>();
        _playerScript = Chractor.GetComponent<PlayerBehaviour>();
        hp = Chractor.GetComponent<Health>();
        _abillity = Chractor.GetComponent<HeroAbility>();
    }

    private void OnEnable()
    {
        //Debug.Log($"플레이어 랜더러 : {hp.isDeath}");
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (!hp.isDeath)
            {
                AliveMotion();
                MoveAniMotion();
                CombatMotion();
            }
        }
    }


    public void DieMotion()
    {
        if (hp.isDeath)
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Die", true);
        }
    }

    public void AliveMotion()
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
 
    private void CombatMotion()
    {
        if (_abillity.isActive == true)
        {
            animator.SetBool("Attack", false);
            return;
        }

        if (_playerScript.perfomMeleeAttack == true || _playerScript.perfomRangeAttack == true)
        {
            if (photonView.IsMine)
            {
                IsAttack = true;
            }
            
        }
        else if (_playerScript.perfomMeleeAttack == false || _playerScript.perfomRangeAttack == false)
        {
            if (photonView.IsMine)
            {
                IsAttack = false;
            }
        }

        animator.SetBool("Attack", IsAttack);
        animator.SetFloat("AttackSpeed", playerStats.attackSpeed);
    }

    public void DizzyMotion(bool stun)
    {
        animator.SetBool("Attack", !stun);
        animator.SetBool("isStun", stun);
    }
}
