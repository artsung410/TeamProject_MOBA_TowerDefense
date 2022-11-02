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
    public bool Targeton = false;
  public  enum ESTATE
    {
        move,
        attack
    }
    public ESTATE _estate;
    public enum EMINIOMTYPE
    {
        Nomal,
        Shot,
        Special,
    }
    public EMINIOMTYPE _eminiomtype;

    float distance;

    protected override void Awake()
    {
        base.Awake();   
        _estate = ESTATE.move;
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
                _PrevTarget = _target; // 
            }
        }
    }
    private void Start()
    {

        StartCoroutine(StateChange());
        InvokeRepeating("UpdateEnemyTarget", 0f, 0.5f);
        if (_eminiomtype == EMINIOMTYPE.Nomal)
        {
            Debug.Log("노멀");
            attackRange = 4f;
        }
        else if (_eminiomtype == EMINIOMTYPE.Shot)
        {
            Debug.Log("원거리");
            attackRange = 10f;
        }

        _navMeshAgent.SetDestination(_target.position); // 넥서스 좌표
        _navMeshAgent.speed = 5f;
    }
    private IEnumerator move() // 움직임  //목표지점까지 움직인다 . 타켓발견 -> 멈춰서 공격 -> 타켓 죽음 -> 타겟변경 -> 타
    {

        while (_estate == ESTATE.move)
        {
            if (_target == null) //타켓이 죽엇을때 공격범위 초기화 
            {
                if (_eminiomtype == EMINIOMTYPE.Nomal) //공격범위 초기화
                {
                    attackRange = 4f;
                }
                if (_eminiomtype == EMINIOMTYPE.Special)
                {
                    attackRange = 6f;
                }
                Targeton = false;
                _target = _PrevTarget;
                _navMeshAgent.SetDestination(_target.position);
            }
            transform.LookAt(new Vector3(_target.position.x,0,_target.position.z)); // 타켓을 바라봄
            Vector3 vecDistance = _target.position - transform.position; //거리계산
            float distance = vecDistance.sqrMagnitude; // 최적화
            if (distance <= attackRange * attackRange) //최적화 공격범위 안에있을때
            {
                _estate = ESTATE.attack; // 어택으로 전환
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
                if (_eminiomtype == EMINIOMTYPE.Nomal) //공격범위 초기화
                {
                    attackRange = 4f;
                }
                if (_eminiomtype == EMINIOMTYPE.Special)
                {
                    attackRange = 6f;
                }
                Targeton = false;
                _target = _PrevTarget;
                _navMeshAgent.SetDestination(new Vector3(_target.position.x, 0, _target.position.z));
            }

            Vector3 vecAtkDistance = _target.position - transform.position;
            float AtkDistance = Vector3.SqrMagnitude(vecAtkDistance);
            _navMeshAgent.isStopped = true;
            _animator.SetBool("Attack", true);
            //Debug.Log($"{AtkDistance},{attackRange * attackRange}");
            transform.LookAt(new Vector3(_target.position.x, 0, _target.position.z));
            // 애니메이션 추가 + 공격데미지 입히기
            //공격쿨타임
            if (AtkDistance >= attackRange * attackRange)
            {
                _animator.SetBool("Attack", false);
                _navMeshAgent.isStopped = false;
                Targeton = false;
                _target = _PrevTarget;
                _navMeshAgent.SetDestination(_target.position);
                _estate = ESTATE.move;
                break;
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
            yield return null; ;
        }

    }
    private void UpdateEnemyTarget() // 타워 6 플레이어 7 미니언 8 12 넥서스 13 스페셜
    {
        Collider[] RangeTarget = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider collider in RangeTarget)
        {
            if (collider.tag == myTag)
            {
                continue;
            }
            if (collider.CompareTag(EnemyTag)) // 범위안에 적 발견
            {

                if (_eminiomtype == EMINIOMTYPE.Special) //특수미니언일때
                {
                    if (Targeton == false && collider.gameObject.layer == 6 || collider.gameObject.layer == 12) //타워랑 넥서스만 공격가능
                    {
                        if (collider.gameObject.layer == 12)
                        {
                            attackRange = 12f;
                        }
                        Targeton = true;
                        _target = collider.transform;
                        _navMeshAgent.SetDestination(_target.position);
                    }
                }
                else //나머지 미니언일때
                {
                    if (Targeton == false && collider.gameObject.layer == 6 || collider.gameObject.layer == 7 || collider.gameObject.layer == 8 || collider.gameObject.layer == 12 || collider.gameObject.layer == 13)
                    {
                        if (_eminiomtype == EMINIOMTYPE.Nomal && collider.gameObject.layer == 6)
                        {
                            attackRange = 6f;
                        }

                        if (collider.gameObject.layer == 12)
                        {
                            attackRange = 12f;
                        }
                        Targeton = true;
                        _target = collider.transform;
                        _navMeshAgent.SetDestination(_target.position);
                    }


                }
                //레이어로 확인해서 공격타켓 설정

            }
        }
    }
}


