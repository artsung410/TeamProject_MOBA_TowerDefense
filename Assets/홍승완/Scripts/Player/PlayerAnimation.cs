using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimation : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    NavMeshAgent agent;
    Animator animator;
    HeroCombat combat;
    Stats playerStats;

    float motionSmoothTime = 0.1f;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        combat = GetComponent<HeroCombat>();
        playerStats = GetComponent<Stats>();
    }
    
    void Start()
    {

    }

    void Update()
    {
        MoveAniMotion();
        CombatMotion();
    }

    private void MoveAniMotion()
    {
        float speed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("Speed", speed, motionSmoothTime, Time.deltaTime);
    }

    IEnumerator MeleeAttackInterval()
    {
        yield return new WaitForSeconds(2f);
        combat.perfomMeleeAttack = false;
        Debug.Log("check");
    }

    float attackAnimationPose = 0;
    private void CombatMotion()
    {
        //Debug.Log(randomPose);
        if (combat.targetedEnemy != null)
        {
            if (combat.perfomMeleeAttack == true)
            {
                // ���� ��� ���
                animator.SetBool("Attack", true);

                // ������ ���� �ڼ�
                animator.SetFloat("AttackPose", attackAnimationPose);

                // ���� ��� ��� �ӵ�
                animator.SetFloat("AttackSpeed", playerStats.attackSpeed);
                StartCoroutine(MeleeAttackInterval());

            }
        }
        else
        {
            if (combat.perfomMeleeAttack == false)
            {
                //Debug.Log("���� ���");
                animator.SetBool("Attack", false);
                StopCoroutine(MeleeAttackInterval());
            }
        }
    }

    
}
