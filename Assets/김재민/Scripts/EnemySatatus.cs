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
    public Transform _PrevTarget { get; private set; }
    public bool Targeton = false;


    public enum ESTATE
    {
        move,
        attack
    }
    public ESTATE _estate;

    float distance;
    float sencerArea;
    float maxArea = 15f;
    private IEnumerator Imove;
    private IEnumerator Iattack;
    private IEnumerator IstateChange;
    protected override void Awake()
    {
        base.Awake();
        _estate = ESTATE.move;
        CurrnetHP = HP;
        Mathf.Clamp(sencerArea, 0f, maxArea);

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag(EnemyTag);
        foreach (GameObject Enemy in Enemys)
        {
            if (Enemy.layer == 14)
            {

                _target = Enemy.transform; // 우물의 위치를 타켓으로 할당
                _PrevTarget = _target; // 
            }
        }
    }
    private void Start()
    {
        _navMeshAgent.SetDestination(_PrevTarget.position); // 넥서스 좌표
        _navMeshAgent.speed = 5f;
        moveSpeed = _navMeshAgent.speed;
        stateChange();
    }


    private void FixedUpdate()
    {

        if (isDead) return;
        UpdateEnemyTarget();
    }

    private IEnumerator move() // 움직임  //목표지점까지 움직인다 . 타켓발견 -> 멈춰서 공격 -> 타켓 죽음 -> 타겟변경 -> 타
    {

        while (isDead == false && _estate == ESTATE.move)
        {
            if (_target == null) //타켓이 죽엇을때 공격범위 초기화 
            {
                attackRange = minionDB.Range;
                _animator.SetBool("Attack", false);
                _navMeshAgent.isStopped = false;
                Targeton = false;
                _target = _PrevTarget;
                transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));

            }
            _navMeshAgent.SetDestination(_target.position);
            transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z)); // 타켓을 바라봄
            Vector3 vecDistance = _target.position - transform.position; //거리계산
            float distance = vecDistance.sqrMagnitude; // 최적화
            if (distance <= attackRange * attackRange) //최적화 공격범위 안에있을때
            {
                _estate = ESTATE.attack; // 어택으로 전환
                stateChange();
                break;
            }
            yield return null;
        }
    }
    private IEnumerator attack() // 공격
    {
        while (isDead == false && _estate == ESTATE.attack)
        {

            if (_target == null)
            {

                _estate = ESTATE.move;
                stateChange();
                break;

            }

            Vector3 vecAtkDistance = _target.position - transform.position;
            float AtkDistance = Vector3.SqrMagnitude(vecAtkDistance);
            //_navMeshObstacle.carving = true;
            _navMeshAgent.isStopped = true;
            _animator.SetBool("Attack", true);
            transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
            // 애니메이션 추가 + 공격데미지 입히기
            //공격쿨타임
            if (AtkDistance > attackRange * attackRange)
            {
                Targeton = false;
                _animator.SetBool("Attack", false);
                _navMeshAgent.isStopped = false;
                //_navMeshObstacle.carving = false;
                _estate = ESTATE.move;
                _target = _PrevTarget;
                stateChange();
                break;
            }
            yield return null;
        }
    }
    private void stateChange()
    {

        switch (_estate)
        {
            case ESTATE.attack:
                StartCoroutine(attack());
                Iattack = attack();
                StopCoroutine(Imove);
                break;
            default:
                StartCoroutine(move());
                Imove = move();
                if (Iattack != null)
                {
                    StopCoroutine(Iattack);
                }
                break;
        }

    }
    private void UpdateEnemyTarget() // 타워 6 플레이어 7 미니언 8 12 넥서스 13 스페셜
    {
        if (Targeton || sencerArea >= maxArea)
        {
            sencerArea = 0f;
            return;
        }
        if (_eminontpye == EMINIONTYPE.Shot)
        {
            sencerArea = 14f;
        }
        sencerArea += 0.1f;
        Collider[] RangeTarget = Physics.OverlapSphere(transform.position, sencerArea);
        foreach (Collider collider in RangeTarget)
        {
            if (collider.tag == myTag || collider.gameObject.layer == 0)
            {
                continue;
            }
            if (collider.CompareTag(EnemyTag)) // 범위안에 적 발견
            {

                if (Targeton == false && collider.gameObject.layer == 6 || collider.gameObject.layer == 7 || collider.gameObject.layer == 8 || collider.gameObject.layer == 12 || collider.gameObject.layer == 13)
                {
                    Targeton = true;
                    _target = collider.transform;
                    transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));


                }

                if (collider.gameObject.layer == 12) // 넥서스 사거리
                {
                    attackRange = 15f; 
                }
                else  if (collider.gameObject.layer == 6)
                {
                    if(_eminontpye == EMINIONTYPE.Nomal)
                    {
                        attackRange = 6f; // 타워사거리 
                    }
                }
                
            }
            else if (collider.gameObject.layer == 17) // 중립몬스터 사거리
            {
                Targeton = true;
                _target = collider.transform;
                transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
                if (_eminontpye == EMINIONTYPE.Nomal)
                {
                    attackRange = 7f;
                }
            }
            //레이어로 확인해서 공격타켓 설정
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sencerArea);
    }
}





