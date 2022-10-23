using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemySatatus : Enemybase
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    public Transform _target;
    private Transform _PrevTarget;

    enum ESTATE
    {
        move,
        attack
    }
    ESTATE _estate;
    enum EMINIOMTYPE
    {
        Nomal,
        Shot,
    }
    EMINIOMTYPE _eminiomtype;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    float distance;

    void Awake()
    {
        _estate = ESTATE.move;
        _eminiomtype = EMINIOMTYPE.Nomal;
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _PrevTarget = _target;
        _navMeshAgent.enabled = false;
        _navMeshAgent.enabled = true;
        CurrnetHP = HP;
    }

    private void Start()
    {
        
        if (gameObject.GetComponents<BulletSpawn>() != null)
        {
            _eminiomtype = EMINIOMTYPE.Shot;
            attackRange = 10f;
        }

        
        StartCoroutine(StateChange());
    }

    private IEnumerator move() // 움직임  //목표지점까지 움직인다 . 타켓발견 -> 멈춰서 공격 -> 타켓 죽음 -> 타겟변경 -> 타
    {
        while (true)
        {
           
            //UpdateEnemyTarget();
            if (_navMeshAgent.enabled == false)
            {
                break;
            }
            _navMeshAgent.speed = 5f;
            _navMeshAgent.SetDestination(_target.position);
            transform.LookAt(_target.position);
            distance = Vector3.Distance(_target.position, transform.position);
            if (distance <= attackRange)
            {
                _estate = ESTATE.attack;

                break;
            }
            yield return null;

        }
    }
    private IEnumerator Attack() // 공격
    {
        while (true)
        {
            
            if (_target == null)
            {
                _target = _PrevTarget;
            }
            float AttackDistance = Vector3.Distance(transform.position, _target.position);
            if (_navMeshAgent.enabled == false)
            {
                break;
            }
            // 구분
            if (_eminiomtype == EMINIOMTYPE.Shot)
            {
                _navMeshAgent.stoppingDistance = attackRange;
            }
            else
            {
                _navMeshAgent.stoppingDistance = 0f;
            }
            _navMeshAgent.speed = 1f;
            transform.LookAt(_target.position);
            _animator.SetBool("Attack", true);
            // 애니메이션 추가 + 공격데미지 입히기
            yield return new WaitForSeconds(AttackTime); //공격쿨타임

            if (AttackDistance >= attackRange)
            {
                _navMeshAgent.isStopped = false;
                _animator.SetBool("Attack", false);
                _estate = ESTATE.move;

            }
            yield return null;
        }
    }

    private IEnumerator StateChange()
    {
        while (true)
        {

            switch (_estate)
            {
                case ESTATE.attack:
                    StartCoroutine(Attack());
                    StopCoroutine(move());
                    break;
                default:
                    StartCoroutine(move());
                    StopCoroutine(Attack());
                    break;

            }
            yield return null;
        }

    }
    private void Update()
    {
        UpdateEnemyTarget();
    }
    private void UpdateEnemyTarget() // 타워 6 플레이어 7 미니언 8
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag(EnemyTag); //tag로 게임오브젝트를 찾고 에너미스에 넣어주고
        float shortestDistance = Mathf.Infinity; //가장가까운 범위

        foreach (GameObject enemy in Enemies) // 에너미들은 다 확인하면서
        {   
            float NearDistance = Vector3.Distance(transform.position, enemy.transform.position); //거리를 구해주고
           
                if (NearDistance <= shortestDistance) // 가장 가까운 타켓
                {
                    // 미니언 우선타격 -> 미니언 없으면 플레이어 -> 타워 때리는 조건은 미니언 없거나 플레이어가 없으면 감지범위안에
                    if (enemy.layer == 8) //미니언
                    {
                        _target.position = enemy.transform.position;
                        
                    }
                    else // 미니언이 죽으면 
                    {
                        _target.position = enemy.transform.position;
                    }
                }

        }
    }
}

