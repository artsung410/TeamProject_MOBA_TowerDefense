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

    public Transform _target; // 타켓
    private Transform _PrevTarget; //넥서스
    Rigidbody _rigidbody;
    enum ESTATE
    {
        move,
        attack
    }
    ESTATE _estate;
    enum EMinionType
    {
        meele,
        shot,
    }
    EMinionType _minionType;
    public bool canAttack { get; private set; }
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private string _targetTag;
    
    float distance;

    void Awake()
    {   
        _estate = ESTATE.move;
        _minionType = EMinionType.meele;
           _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _PrevTarget = _target;
        _navMeshAgent.enabled = false;
        _navMeshAgent.enabled = true;


    }

    private void Start()
    {

        if(attackRange > 5f) // 근접 , 원거리 구분
        {
            _minionType = EMinionType.shot;
        }
       
        StartCoroutine(StateChange());
    }

    private IEnumerator move() // 움직임
    {
        while (true)
        {
            if (_target == null)
            {
                _target = _PrevTarget;
            }
            _animator.SetBool("Attack",false);
            _navMeshAgent.SetDestination(_target.position);
            transform.LookAt(_target.position);
            distance = Vector3.Distance(_target.position, transform.position);
            if (distance <= attackRange) // 거리가 공격사거리보다 같거나적으면
            {
                _estate = ESTATE.attack; // 공격상태
              

                break;
            }
            yield return null;

        }
    }
    private IEnumerator Attack() // 공격
    {
        while (true)
        {
           float AttackDistance = Vector3.Distance(transform.position,_target.position); //공격중일때 사거리
            if (_navMeshAgent.enabled == false)
            {
                break;
            }
            // 구분
            if (_target == null)
            {
                _target = _PrevTarget;
            }
            if(_minionType == EMinionType.shot)
            {
                _navMeshAgent.stoppingDistance = AttackDistance;

            }
            else
            {
                _navMeshAgent.stoppingDistance = 0f;
            }
            _navMeshAgent.isStopped = true;

            _animator.SetBool("Attack",true); //공격모션
            // 애니메이션 추가 + 공격데미지 입히기
            //yield return new WaitForSeconds(1f); //공격쿨타임

                if (AttackDistance >= attackRange) //공격중일때 공격사거리 벗어나면 move상태로 전환
                {
                _navMeshAgent.isStopped = false;
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

    private void UpdateTarget()
    {
        GameObject[] enemise = GameObject.FindGameObjectsWithTag(_targetTag);
        foreach (GameObject enemy in enemise)
        {
            float NearEnemyDistance = Vector3.Distance(transform.position,enemy.transform.position);
            if(NearEnemyDistance <= attackRange)
            {
                _target = enemy.transform;
            }
        }
    
    }
    


}

