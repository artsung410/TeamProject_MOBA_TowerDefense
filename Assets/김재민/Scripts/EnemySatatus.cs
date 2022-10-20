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

    public Transform _target; // Ÿ��
    private Transform _PrevTarget; //�ؼ���
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

        if(attackRange > 5f) // ���� , ���Ÿ� ����
        {
            _minionType = EMinionType.shot;
        }
       
        StartCoroutine(StateChange());
    }

    private IEnumerator move() // ������
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
            if (distance <= attackRange) // �Ÿ��� ���ݻ�Ÿ����� ���ų�������
            {
                _estate = ESTATE.attack; // ���ݻ���
              

                break;
            }
            yield return null;

        }
    }
    private IEnumerator Attack() // ����
    {
        while (true)
        {
           float AttackDistance = Vector3.Distance(transform.position,_target.position); //�������϶� ��Ÿ�
            if (_navMeshAgent.enabled == false)
            {
                break;
            }
            // ����
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

            _animator.SetBool("Attack",true); //���ݸ��
            // �ִϸ��̼� �߰� + ���ݵ����� ������
            //yield return new WaitForSeconds(1f); //������Ÿ��

                if (AttackDistance >= attackRange) //�������϶� ���ݻ�Ÿ� ����� move���·� ��ȯ
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

