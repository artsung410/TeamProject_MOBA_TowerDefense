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
    [HideInInspector]
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
        Special,
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

        _navMeshAgent.enabled = false;
        _navMeshAgent.enabled = true;
        CurrnetHP = HP;

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag(EnemyTag);
        foreach (GameObject Enemy in Enemys)
        {
            if (Enemy.layer == 12)
            {
                _target = Enemy.transform;
                _PrevTarget = _target;
            }
        }
    }

    private void Start()
    {

        StartCoroutine(StateChange());
        InvokeRepeating("UpdateEnemyTarget", 0f, 1f);
        if (_eminiomtype == EMINIOMTYPE.Nomal && _eminiomtype == EMINIOMTYPE.Special) 
        {
            attackRange = 2f;
        }
        else if (_eminiomtype == EMINIOMTYPE.Shot)
        {
            attackRange = 10f;
        }
        
        _navMeshAgent.speed = 5f;
    }

    private IEnumerator move() // 움직임  //목표지점까지 움직인다 . 타켓발견 -> 멈춰서 공격 -> 타켓 죽음 -> 타겟변경 -> 타
    {
        while (_estate == ESTATE.move)
        {
            if (_target == null)
            {
                Targeton = false;
                _target = _PrevTarget;
            }
            if (_navMeshAgent.enabled == false)
            {
                break;
            }
            _navMeshAgent.SetDestination(_target.position);
            transform.LookAt(_target.position);
            Vector3 vecDistance = _target.position - transform.position;
            float distance = vecDistance.sqrMagnitude;
            if (distance <= attackRange * attackRange)
            {
                _estate = ESTATE.attack;

                break;
            }
            yield return null;
        }
    }
    private IEnumerator Attack() // 공격
    {
        while (_estate == ESTATE.attack)
        {
            if (_target == null)
            {
                Targeton = false;
                _target = _PrevTarget;
            }
            Vector3 VceAtkdistance = _target.position - transform.position;
            float AtkDistance = Vector3.SqrMagnitude(VceAtkdistance);
            // 구분
            _navMeshAgent.isStopped = true;
            _animator.SetBool("Attack", true);
            transform.LookAt(_target.position);
            // 애니메이션 추가 + 공격데미지 입히기
            //공격쿨타임
            if (AtkDistance >= attackRange * attackRange)
            {
                _estate = ESTATE.move;
                _animator.SetBool("Attack", false);
                _navMeshAgent.isStopped = false;
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
            Debug.Log($"{_estate}");
            yield return null; ;
        }

    }
    private void UpdateEnemyTarget() // 타워 6 플레이어 7 미니언 8
    {
        Targeton = false;
        Collider[] RangeTarget = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider collider in RangeTarget)
        {
            if (collider.tag == myTag)
            {
                continue;

            }
            if (collider.CompareTag(EnemyTag))
            {
                if (collider.gameObject.layer == 8 && Targeton == false && _eminiomtype != EMINIOMTYPE.Special) //미니언 공격 특수미니언 아닐때
                {
                    Targeton = true;
                    _target = collider.transform;
                }
                else if (collider.gameObject.layer == 7 && Targeton == false && _eminiomtype != EMINIOMTYPE.Special) // 플레이어 공격 특수 미니언 아닐때
                {
                    Targeton = true;
                    _target = collider.transform;
                }
                else if (collider.gameObject.layer == 6 && Targeton == false)// 타워플레이어 공격
                {
                    if (_eminiomtype == EMINIOMTYPE.Nomal || _eminiomtype == EMINIOMTYPE.Special) //특수 미니언이나 노멀미니언만
                    {
                        attackRange = 6f;
                    }
                    Targeton = true;
                    _target = collider.transform;
                }
                else if (collider.gameObject.layer == 13 && Targeton == false && _eminiomtype != EMINIOMTYPE.Special)  //특수무니언 공격
                {
                    _target = collider.transform;
                    Targeton = true;
                }
                else if (collider.gameObject.layer == 12 && Targeton == false) // 넥서스
                {
                    _target = collider.transform;
                    Targeton = true;
                }
            }
        }
    }
}

