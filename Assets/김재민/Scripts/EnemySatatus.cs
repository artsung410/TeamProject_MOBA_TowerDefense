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
                _PrevTarget = _target; // 
            }
        }
    }
    private void Start()
    {

        StartCoroutine(StateChange());
        InvokeRepeating("UpdateEnemyTarget", 0f, 0.5f);
        if(_eminiomtype == EMINIOMTYPE.Nomal)
        {
            Debug.Log("���");
            attackRange = 3f;
        }
        else if (_eminiomtype == EMINIOMTYPE.Shot)
        {
            Debug.Log("���Ÿ�");
            attackRange = 10f;
        }
        else
        {
            Debug.Log("Ư��");
            attackRange = 6f;
        }
       
        _navMeshAgent.SetDestination(_target.position); // �ؼ��� ��ǥ
        _navMeshAgent.speed = 5f;
    }
    private IEnumerator move() // ������  //��ǥ�������� �����δ� . Ÿ�Ϲ߰� -> ���缭 ���� -> Ÿ�� ���� -> Ÿ�ٺ��� -> Ÿ
    {
        while (_estate == ESTATE.move)
        {
            if (_target == null) //Ÿ���� �׾����� ���ݹ��� �ʱ�ȭ 
            {
                if (_eminiomtype == EMINIOMTYPE.Nomal) //���ݹ��� �ʱ�ȭ
                {
                    attackRange = 3f;
                }
                if (_eminiomtype == EMINIOMTYPE.Special)
                {
                    attackRange = 6f;
                }
                Targeton = false;
                _target = _PrevTarget;
                _navMeshAgent.SetDestination(_target.position);
            }
            transform.LookAt(_target.position); // Ÿ���� �ٶ�
            Vector3 vecDistance = _target.position - transform.position; //�Ÿ����
            float distance = vecDistance.sqrMagnitude; // ����ȭ
            if (distance <= attackRange * attackRange) //����ȭ ���ݹ��� �ȿ�������
            {
                _estate = ESTATE.attack; // �������� ��ȯ
                break;
            }
            yield return null;
        }
    }
    private IEnumerator Attack() // ����
    {
        while (_estate == ESTATE.attack)
        {
            if (_target == null)
            {
                if (_eminiomtype == EMINIOMTYPE.Nomal) //���ݹ��� �ʱ�ȭ
                {
                    attackRange = 3f;
                }
                if (_eminiomtype == EMINIOMTYPE.Special)
                {
                    attackRange = 6f;
                }
                Targeton = false;
                _target = _PrevTarget;
                _navMeshAgent.SetDestination(_target.position);
            }

            Vector3 vceAtkDistance = _target.position - transform.position;
            float AtkDistance = Vector3.SqrMagnitude(vceAtkDistance);
            _navMeshAgent.isStopped = true;
            _animator.SetBool("Attack", true);
            transform.LookAt(_target.position);
            // �ִϸ��̼� �߰� + ���ݵ����� ������
            //������Ÿ��
            if (AtkDistance >= attackRange * attackRange)
            {
                _estate = ESTATE.move;
                _animator.SetBool("Attack", false);
                _navMeshAgent.isStopped = false;
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
    private void UpdateEnemyTarget() // Ÿ�� 6 �÷��̾� 7 �̴Ͼ� 8 12 �ؼ��� 13 �����
    {

        Collider[] RangeTarget = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider collider in RangeTarget)
        {
            if (collider.tag == myTag)
            {
                continue;
            }
            if (collider.CompareTag(EnemyTag)) // �����ȿ� �� �߰�
            {

                if (_eminiomtype == EMINIOMTYPE.Special) //Ư���̴Ͼ��϶�
                {
                    if (Targeton == false && collider.gameObject.layer == 6 || collider.gameObject.layer == 12) //Ÿ���� �ؼ����� ���ݰ���
                    {
                        if (collider.gameObject.layer == 12 && _eminiomtype == EMINIOMTYPE.Special)
                        {
                            attackRange = 13f;
                        }
                        Targeton = true;
                        _target = collider.transform;
                        _navMeshAgent.SetDestination(_target.position);
                    }
                }
                else //������ �̴Ͼ��϶�
                {
                    if (Targeton == false && collider.gameObject.layer == 6 || collider.gameObject.layer == 7 || collider.gameObject.layer == 8 || collider.gameObject.layer == 12 || collider.gameObject.layer == 13)
                    {
                        if (collider.gameObject.layer == 12 && _eminiomtype == EMINIOMTYPE.Nomal)
                        {
                            attackRange = 13f;
                        }
                        Targeton = true;
                        _target = collider.transform;
                        _navMeshAgent.SetDestination(_target.position);
                    }
                }


            }
            //���̾�� Ȯ���ؼ� ����Ÿ�� ����

        }
    }
}


