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
    private bool Targeton = false;

    enum ESTATE
    {
        move,
        attack
    }
    ESTATE _estate;
    public enum EMINIOMTYPE
    {
        Nomal,
        Shot,
    }
    public EMINIOMTYPE _eminiomtype;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    float distance;

    void Awake()
    {
        _estate = ESTATE.move;
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _PrevTarget = _target;
        _navMeshAgent.enabled = false;
        _navMeshAgent.enabled = true;
        CurrnetHP = HP;

    }

    private void Start()
    {
        

        StartCoroutine(StateChange());
        InvokeRepeating("UpdateEnemyTarget", 0f, 1f);
    }

    private IEnumerator move() // 움직임  //목표지점까지 움직인다 . 타켓발견 -> 멈춰서 공격 -> 타켓 죽음 -> 타겟변경 -> 타
    {
        while (true)
        {
            if (_eminiomtype == EMINIOMTYPE.Nomal)
            {
                attackRange = 2f;
            }
            else if (_eminiomtype == EMINIOMTYPE.Shot)
            {
                attackRange = 10f;
            }
            if (_target == null)
            {
                _target = _PrevTarget;
            }
            _animator.SetBool("Attack", false);
            _navMeshAgent.isStopped = false;
            if (_navMeshAgent.enabled == false)
            {
                break;
            }
          
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
                Debug.Log($"{_estate}");

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
            float AttackDistance = Vector3.Distance(_target.position, transform.position);
            Debug.Log($"{attackRange}");
            if (_navMeshAgent.enabled == false)
            {
                break;
            }
            // 구분
            _navMeshAgent.isStopped = true;
            _animator.SetBool("Attack", true);
            transform.LookAt(_target.position);
            
            // 애니메이션 추가 + 공격데미지 입히기
            yield return new WaitForSeconds(AttackTime); //공격쿨타임
            Debug.Log($" 공격중 사거리:{AttackDistance}, 공격범위 : {attackRange}");
            if (AttackDistance >= attackRange)
            {
                _estate = ESTATE.move;
                Targeton = false;
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
    private void UpdateEnemyTarget() // 타워 6 플레이어 7 미니언 8
    {
        if (_target == null)
        {
            Targeton = false;
            _target = _PrevTarget;

        }
        Collider[] RangeTarget = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider collider in RangeTarget)
        {
            if (collider.tag == myTag)
            {
                continue;
          
            }
            if (collider.CompareTag(EnemyTag))
            {
                if (collider.gameObject.layer == 8 && Targeton == false) //미니언 공격
                {
                    Targeton = true;
                    _target = collider.transform;
                }
                else if (collider.gameObject.layer == 7 && Targeton == false) // 플레이어 공격
                {
                    Targeton = true;
                    _target = collider.transform;
                }
                else if (collider.gameObject.layer == 6 && Targeton == false)// 타워플레이어 공격
                {
                    if(_eminiomtype == EMINIOMTYPE.Nomal)
                    {
                        attackRange = 6f;
                    }
                    Targeton = true;
                    _target = collider.transform;
                }
                else if(collider.gameObject.layer == 5 && Targeton == false)
                {
                    _target = collider.transform;
                    Targeton = true;
                }
            }
        }
    }
}

