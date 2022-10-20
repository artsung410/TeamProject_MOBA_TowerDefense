using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public enum HeroAttackType
{
    Melee,
    Ranged,
}

public class PlayerBehaviour : MonoBehaviourPun
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


    public string EnemyTag;


    #region Other Components
    NavMeshAgent _agent;
    Stats _statScript;
  

    #endregion

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _statScript = GetComponent<Stats>();
        _agent.enabled = false;
        _agent.enabled = true;
    }

    private void Start()
    {
        //PhotonView photonView = PhotonView.Get(this);
        //photonView.RPC("RPCStorageCaller", RpcTarget.MasterClient, playerStorage._id, playerStorage.session_id, playerStorage.userName, playerStorage.playerNumber, playerStorage.zera, playerStorage.ace, playerStorage.bet_id);

        // ȣ��Ʈ������ Blue�±� �Ҵ�
        
            gameObject.tag = "Blue";
            EnemyTag = "Red";

            // �ٸ� Ŭ���̾�Ʈ ChangeTag ����
            photonView.RPC("ChangeTag", RpcTarget.Others);

    }


    [PunRPC]
    private void ChangeTag()
    {
        gameObject.tag = "Red";
        EnemyTag = "Blue";
    }

    private void Update()
    {
        // �÷��̾� ��ġ���� ī�޶�� ����
        if (photonView.IsMine)
        {
            CurrentPlayerPos = transform.position;
            _agent.speed = _statScript.MoveSpeed;
            MoveTo();

            // sŰ ������ ����
            if (Input.GetKeyDown(KeyCode.S))
            {
                _agent.SetDestination(CurrentPlayerPos);
                _agent.stoppingDistance = 0f;
            }
        }
    }

    public void MoveTo()
    {

        HeroAliveCheck();

        // ��Ŭ���� �̵�
        if (Input.GetMouseButton(1))
        {
            // ���콺��ġ���� �� raycast�� ��ü�� �´´ٸ�, �װ��� navmesh��������
            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out Hit, Mathf.Infinity))
            {
                //Debug.DrawLine(Cam.ScreenPointToRay(Input.mousePosition).origin, Cam.ScreenPointToRay(Input.mousePosition).direction * Mathf.Infinity, Color.blue, 1f);

                MoveOntheGround(Hit);
                GetTargetedObject();
            }
        }

        MoveEnemyPosition();
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
        // Ÿ���� ������
        if (targetedEnemy != null)
        {
            float dist = Vector3.Distance(transform.position, targetedEnemy.transform.position) - 0.5f;

            // Ÿ���� ���ݹ��� ���϶�
            if (dist > _statScript.attackRange)
            {
                // �� ��ġ�� �̵��Ѵ�
                _agent.SetDestination(targetedEnemy.transform.position);
                _agent.stoppingDistance = _statScript.attackRange;

                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref RotateVelocity,
                    RotateSpeed * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);
            }
            else
            {
                _agent.SetDestination(targetedEnemy.transform.position);
                _agent.stoppingDistance = _statScript.attackRange;

                // Ÿ���� �ٶ󺻴�
                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref RotateVelocity,
                    RotateSpeed * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);

                // ���� ����ĳ���
                if (heroAttackType == HeroAttackType.Melee)
                {
                    // ���� ���� ����ġ�� true�� �ٲ�
                    perfomMeleeAttack = true;
                    photonView.RPC("IsAttack", RpcTarget.All);
                }
            }

        }
    }

    [PunRPC]
    private void IsAttack()
    {
        perfomMeleeAttack = true;
    }

    //public void MeleeAttack()
    //{
    //    if (targetedEnemy != null)
    //    {

    //        TakeDamage(_statScript.attackDmg);
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}


    //// ���������� ü���� ���ҽ�Ű�� �κ�
    //[PunRPC]
    //void TakeDamage(float damage)
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        _statScript.health -= damage;

    //        photonView.RPC("TakeDamage", RpcTarget.Others, _statScript.attackDmg);
    //    }
    //}

    private void GetTargetedObject()
    {
        if (Hit.collider.CompareTag(EnemyTag))
        {
            targetedEnemy = Hit.collider.gameObject;
        }
        else
        {
            targetedEnemy = null;
        }
    }

}