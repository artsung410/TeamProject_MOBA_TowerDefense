using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum HeroAttackType
{
    Melee,
    Ranged,
}

public class PlayerBehaviour : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public static Vector3 CurrentPlayerPos;

    [Header("���� ���")]
    public HeroAttackType heroAttackType;

    [Header("ȸ�� �ӵ�")]
    [SerializeField] float RotateSpeed = 0.5f;
    public float RotateVelocity;

    [Header("���̾��(Ȯ�强 ����Ͽ� �߰���)")]
    public LayerMask Layer;
    RaycastHit Hit;

    // ���� ���� ī�޶�
    public Camera Cam;

    [Header("Ÿ��")]
    public GameObject targetedEnemy;

    [Header("�������ΰ�")]
    public bool perfomMeleeAttack = false;



    #region Other Components
    NavMeshAgent _agent;
    Stats _statScript;

    #endregion

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _statScript = GetComponent<Stats>();
    }


    private void Update()
    {
        // �÷��̾� ��ġ���� ī�޶�� ����
        CurrentPlayerPos = transform.position;

        HeroAliveCheck();

        // ��Ŭ���� �̵�
        if (Input.GetMouseButton(1))
        {
            // ���콺��ġ���� �� raycast�� ��ü�� �´´ٸ�, �װ��� navmesh��������
            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out Hit, Mathf.Infinity))
            {
                Debug.DrawLine(Cam.ScreenPointToRay(Input.mousePosition).origin, Cam.ScreenPointToRay(Input.mousePosition).direction * Mathf.Infinity, Color.blue, 1f);

                MoveOntheGround(Hit);
                GetTargetedObject();
                MoveEnemyPosition();
            }
        }
    }


    /// <summary>
    /// ���� ����ִ��� Ȯ���ϴ� �޼���
    /// </summary>
    public void HeroAliveCheck()
    {
        //if (targetedEnemy != null)
        //{
        //    if (targetedEnemy.GetComponent<HeroCombat>() != null)
        //    {
        //        if (targetedEnemy.GetComponent<HeroCombat>().isHeroAlive)
        //        {
        //            targetedEnemy = null;
        //        }
        //    }
        //}
    }

    public void MoveOntheGround(RaycastHit hit)
    {
        // MOVEMENT
        // ��Ŭ���� ������ ������
        _agent.SetDestination(hit.point);
        targetedEnemy = null;
        _agent.stoppingDistance = 0;

        // ROTATION
        Quaternion lookAtTheTarget = Quaternion.LookRotation(hit.point - transform.position);
        // ĳ���Ͱ� Ÿ���� ���� ȸ���ϴ� �ӵ� ����
        // RotateSpeed���� �������� ������ ȸ����
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtTheTarget.eulerAngles.y, ref RotateVelocity, RotateSpeed * (Time.deltaTime * 5));

        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }

    public void MoveEnemyPosition()
    {
        if (targetedEnemy != null)
        {
            if (Vector3.Distance(transform.position, targetedEnemy.transform.position) > _statScript.attackRange)
            {
                _agent.stoppingDistance = _statScript.attackRange;
            }
            else
            {
                _agent.stoppingDistance = Vector3.Distance(transform.position, targetedEnemy.transform.position);

                if (heroAttackType == HeroAttackType.Melee)
                {
                    perfomMeleeAttack = true;
                }
            }

            _agent.SetDestination(targetedEnemy.transform.position);
            Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                rotationToLookAt.eulerAngles.y,
                ref RotateVelocity,
                RotateSpeed * (Time.deltaTime * 5));

            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }
    }

    public void MeleeAttack()
    {
        if (targetedEnemy != null)
        {
            Debug.Log("������ Ÿ���� ����");
        }
        else
        {
            return;
        }
    }

    private void GetTargetedObject()
    {

        if (Hit.collider.GetComponent<Targetable>() != null)
        {
            if (Hit.collider.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Minion)
            {
                targetedEnemy = Hit.collider.gameObject;
            }
        }
        else
        {
            targetedEnemy = null;
        }
    }

}
