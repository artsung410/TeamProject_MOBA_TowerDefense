using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public enum HeroAttackType
    {
        Melee,
        Ranged,
    }
    public HeroAttackType heroAttackType;

    public GameObject targetedEnemy;
    public Animator animator;

    public float attackRange;
    public float rotateSpeedForAttack;

    private PlayerMovement _playerMove;
    private Stats _statScripts;

    //public bool basicAtkIdle = false;
    public bool isHeroAlive;
    public bool perfomMeleeAttack = false;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMovement>();
        _statScripts = GetComponent<Stats>();
        //animator = GetComponent<Animator>().CrossFade("Attack",0,1, _statScripts.attackSpeed)
    }

    void Start()
    {
        
    }

    private void Update()
    {
        //WaitTime();
        MoveEnemyLocation();
    }

    float elapsedTime;

    public void MoveEnemyLocation()
    {
        if (targetedEnemy != null)
        {
            if (Vector3.Distance(gameObject.transform.position, targetedEnemy.transform.position) > attackRange)
            {
                _playerMove._agent.SetDestination(targetedEnemy.transform.position);
                _playerMove._agent.stoppingDistance = attackRange;

                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);
                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref _playerMove.RotateVelocity,
                    rotateSpeedForAttack * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);

            }
            else
            {
                _playerMove._agent.stoppingDistance = attackRange;

                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);
                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref _playerMove.RotateVelocity,
                    rotateSpeedForAttack * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);

                if (heroAttackType == HeroAttackType.Melee)
                {

                    perfomMeleeAttack = true;
                    //elapsedTime += Time.deltaTime;
                    //if (elapsedTime >= (1 / _statScripts.attackSpeed) + 1)
                    //{
                    //    targetedEnemy.GetComponent<Stats>().health -= _statScripts.attackDmg;
                    //    elapsedTime = 0f;
                    //}
                }
            }
        }
        
    }



    public void MeleeAttack()
    {
        if (targetedEnemy != null)
        {
            targetedEnemy.GetComponent<Stats>().health -= _statScripts.attackDmg;
        }
        else
        {
            return;
        }
    }
   


    //IEnumerator MeleeAttackInterval()
    //{
        
    //    perfomMeleeAttack = false;
    //    animator.SetBool("Attack", true);

    //    yield return new WaitForSeconds(_statScripts.attackTime);

    //    //MeleeAttack();

    //    if (targetedEnemy == null)
    //    {
    //        Debug.Log("공격 중단");
    //        //animator.SetBool("Attack", false);
    //        animator.SetBool("Attack", false);

    //        perfomMeleeAttack = true;
    //    }
    //}


    //public void MeleeAttack()
    //{
    //    Debug.Log("공격");
    //    if (targetedEnemy != null)
    //    {
    //        if (targetedEnemy.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Minion)
    //        {

    //            targetedEnemy.GetComponent<Stats>().health -= _statScripts.attackDmg;
    //        }
    //    }

    //    perfomMeleeAttack = true;
    //}

}
