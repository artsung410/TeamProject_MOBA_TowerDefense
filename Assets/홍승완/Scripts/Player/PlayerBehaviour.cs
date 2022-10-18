using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public enum HeroAttackType
{
    Melee,
    Ranged,
}

public class PlayerBehaviour : MonoBehaviourPun
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

    private void Start()
    {
        if (photonView.IsMine)
        {
            gameObject.tag = "Player";
        }
        else
        {
            gameObject.tag = "Enemy";
        }
    }

    private void Update()
    {
        // 플레이어 위치정보 카메라로 보냄
        if (photonView.IsMine)
        {
            CurrentPlayerPos = transform.position;
            MoveTo();
        }
    }

    public void MoveTo()
    {

        HeroAliveCheck();

        // 우클릭시 이동
        if (Input.GetMouseButton(1))
        {
            // 마우스위치에서 쏜 raycast가 물체에 맞는다면, 그곳이 navmesh도착지점
            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out Hit, Mathf.Infinity))
            {
                //Debug.DrawLine(Cam.ScreenPointToRay(Input.mousePosition).origin, Cam.ScreenPointToRay(Input.mousePosition).direction * Mathf.Infinity, Color.blue, 1f);

                MoveOntheGround(Hit);
                GetTargetedObject();
            }
        }

        MoveEnemyPosition();
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
        // 타겟이 있을때
        if (targetedEnemy != null)
        {
            float dist = Vector3.Distance(transform.position, targetedEnemy.transform.position) - 0.5f;

            // 타겟이 공격범위 밖일때
            if (dist > _statScript.attackRange)
            {
                // 그 위치로 이동한다
                _agent.SetDestination(targetedEnemy.transform.position);
                _agent.stoppingDistance = _statScript.attackRange;

                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref RotateVelocity,
                    RotateSpeed * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);
            }
            else
            {
                _agent.SetDestination(targetedEnemy.transform.position);
                _agent.stoppingDistance = _statScript.attackRange;

                // 타겟을 바라본다
                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref RotateVelocity,
                    RotateSpeed * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);

                // 내가 근접캐라면
                if (heroAttackType == HeroAttackType.Melee)
                {
                    // 공격 수행 스위치를 true로 바꿈
                    perfomMeleeAttack = true;
                }
            }

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

        //if (Hit.collider.GetComponent<Targetable>() != null)
        //{
        //    if (Hit.collider.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Minion)
        //    {
        //        targetedEnemy = Hit.collider.gameObject;
        //    }
        //}
        //else
        //{
        //    targetedEnemy = null;
        //}
        if (Hit.collider.GetComponent<PlayerBehaviour>() != null)
        {
            targetedEnemy = Hit.collider.gameObject;
        }
        else
        {
            targetedEnemy = null;
        }
    }

}