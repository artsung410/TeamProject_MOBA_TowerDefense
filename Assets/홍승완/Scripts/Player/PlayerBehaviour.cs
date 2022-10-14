using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum HeroAttackType
{
    Melee,
    Ranged,
}

public class PlayerBehaviour : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public static Vector3 CurrentPlayerPos;

    [Header("공격 방식")]
    public HeroAttackType heroAttackType;

    [Header("회전 속도")]
    [SerializeField] float RotateSpeed = 0.5f;
    public float RotateVelocity;

    [Header("레이어설정(확장성 고려하여 추가함)")]
    public LayerMask Layer;
    RaycastHit Hit;

    // 레이 찍을 카메라
    public Camera Cam;

    [Header("타겟")]
    public GameObject targetedEnemy;

    [Header("공격중인가")]
    public bool perfomMeleeAttack = false;



    #region Other Components
    NavMeshAgent _agent;
    Stats _statScript;

    #endregion

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _statScript = GetComponent<Stats>();
    }


    private void Update()
    {
        // 플레이어 위치정보 카메라로 보냄
        CurrentPlayerPos = transform.position;

        HeroAliveCheck();

        // 우클릭시 이동
        if (Input.GetMouseButton(1))
        {
            // 마우스위치에서 쏜 raycast가 물체에 맞는다면, 그곳이 navmesh도착지점
            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out Hit, Mathf.Infinity))
            {
                Debug.DrawLine(Cam.ScreenPointToRay(Input.mousePosition).origin, Cam.ScreenPointToRay(Input.mousePosition).direction * Mathf.Infinity, Color.blue, 1f);

                MoveOntheGround(Hit);
                GetTargetedObject();
                MoveEnemyPosition();
            }
        }
    }


    /// <summary>
    /// 적이 살아있는지 확인하는 메서드
    /// </summary>
    public void HeroAliveCheck()
    {
        //if (targetedEnemy != null)
        //{
        //    if (targetedEnemy.GetComponent<HeroCombat>() != null)
        //    {
        //        if (targetedEnemy.GetComponent<HeroCombat>().isHeroAlive)
        //        {
        //            targetedEnemy = null;
        //        }
        //    }
        //}
    }

    public void MoveOntheGround(RaycastHit hit)
    {
        // MOVEMENT
        // 우클릭한 지점이 목적지
        _agent.SetDestination(hit.point);
        targetedEnemy = null;
        _agent.stoppingDistance = 0;

        // ROTATION
        Quaternion lookAtTheTarget = Quaternion.LookRotation(hit.point - transform.position);
        // 캐릭터가 타겟을 향해 회전하는 속도 보간
        // RotateSpeed값이 작을수록 빠르게 회전함
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtTheTarget.eulerAngles.y, ref RotateVelocity, RotateSpeed * (Time.deltaTime * 5));

        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }

    public void MoveEnemyPosition()
    {
        if (targetedEnemy != null)
        {
            if (Vector3.Distance(transform.position, targetedEnemy.transform.position) > _statScript.attackRange)
            {
                _agent.stoppingDistance = _statScript.attackRange;
            }
            else
            {
                _agent.stoppingDistance = Vector3.Distance(transform.position, targetedEnemy.transform.position);

                if (heroAttackType == HeroAttackType.Melee)
                {
                    perfomMeleeAttack = true;
                }
            }

            _agent.SetDestination(targetedEnemy.transform.position);
            Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                rotationToLookAt.eulerAngles.y,
                ref RotateVelocity,
                RotateSpeed * (Time.deltaTime * 5));

            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }
    }

    public void MeleeAttack()
    {
        if (targetedEnemy != null)
        {
            Debug.Log("적에게 타격을 입힘");
        }
        else
        {
            return;
        }
    }

    private void GetTargetedObject()
    {

        if (Hit.collider.GetComponent<Targetable>() != null)
        {
            if (Hit.collider.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Minion)
            {
                targetedEnemy = Hit.collider.gameObject;
            }
        }
        else
        {
            targetedEnemy = null;
        }
    }

}
