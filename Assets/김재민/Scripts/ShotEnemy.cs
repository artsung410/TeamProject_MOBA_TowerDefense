using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShotEnemy : Enemybase
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
  
  public  Transform _target;

    public Transform _PrevTarget;
    Rigidbody _rigidbody;
    enum ESTATE
    {
        move,
        attack
    }
    ESTATE _estate;
    public bool canShot { get; private set; }
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    float distance;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _PrevTarget = _target;
        _animator = GetComponent<Animator>();

    }

    private void Start()
    {
        StartCoroutine(StateChange());
    }

    private IEnumerator move() // 움직임
    {
        while (true)
        {
            _navMeshAgent.isStopped = false;
            if (_navMeshAgent.enabled == false)
            {
                break;
            }
            if (_target == null)
            {
                
                _target = _PrevTarget;
            }
            _animator.SetBool("Attack",false);
            canShot = false;
            _navMeshAgent.SetDestination(_target.position);
            transform.LookAt(_target.position);
            distance = Vector3.Distance(_target.position, transform.position);
            if (distance < 10f)
            {
                _estate = ESTATE.attack;
                Debug.Log($"{_estate}");

                break;
            }
            yield return null;

        }
    }
    float AttackDistance;
    private IEnumerator Attack() // 공격
    {
        while (true)
        {
            // 구분

            if (_navMeshAgent.enabled == false)
            {
                break;
            }
            _animator.SetBool("Attack", true);
            canShot = true;
            if(_target == null)
            {
                
                _target = _PrevTarget; 
                
            }

            transform.LookAt(_target.position);
            _navMeshAgent.isStopped = true;

            // 애니메이션 추가 + 공격데미지 입히기
            //yield return new WaitForSeconds(1f); //공격쿨타임
            AttackDistance = Vector3.Distance(transform.position,_target.position);

            if (AttackDistance > 5f)
            {
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

}
