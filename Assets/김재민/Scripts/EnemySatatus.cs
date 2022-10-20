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
    //����� �޷�
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

    }

    private void Start()
    {
      

        if(gameObject.GetComponents<BulletSpawn>() != null)
        {
            _eminiomtype = EMINIOMTYPE.Shot;
            attackRange = 10f;
        }
        
        //InvokeRepeating("UpdateEnemyTarget", 0f, 1f);
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
    private IEnumerator Attack() // ����
    {
        while (true)
        {
           float AttackDistance = Vector3.Distance(transform.position,_target.position);
            if (_navMeshAgent.enabled == false)
            {
                break;
            }
            // ����
            if (_target == null)
            {
                _target = _PrevTarget;
            }
            if (_eminiomtype == EMINIOMTYPE.Shot)
            {
                _navMeshAgent.stoppingDistance = attackRange;
            }
            else
            {
                _navMeshAgent.stoppingDistance = 0f;
            }
            _navMeshAgent.speed = 1f;
            _navMeshAgent.isStopped = true;
            transform.LookAt(_target.position);
            _animator.SetBool("Attack",true);
            // �ִϸ��̼� �߰� + ���ݵ����� ������
            yield return new WaitForSeconds(AttackTime); //������Ÿ��

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
    //private void UpdateEnemyTarget()
    //{
    //    GameObject[] Enemies = GameObject.FindGameObjectsWithTag(EnemyTag); //tag�� ���ӿ�����Ʈ�� ã�� ���ʹ̽��� �־��ְ�
    //    float shortestDistance = Mathf.Infinity; //���尡��� ����
    //    foreach (GameObject enemy in Enemies) // ���ʹ̵��� �� Ȯ���ϸ鼭
    //    {
    //         float NearDistance = Vector3.Distance(transform.position,enemy.transform.position); //�Ÿ��� �����ְ�
    //        if (NearDistance < shortestDistance) // ����
    //        {
    //            _target.position = enemy.transform.position; //Ÿ���� �ٲ��ش�

    //        }
    //    }
    //}

  

   
}

