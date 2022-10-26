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
    //             MAIL : minsub4400@gmail.com   
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
    public bool IsAttack;

    // SMS Start --------------------------------------------//
    // AŰ Ŀ�� ���� ����

    public Texture2D cursorTextureOriginal;
    public Texture2D cursorTexture;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public Canvas moveMouseCanvas;
    public GameObject moveMouseObj;
    public MousePointer moveMousePointer;
    // SMS End --------------------------------------------//

    #region Other Components
    NavMeshAgent _agent;
    Stats _statScript;
    Health _playerHealth;
    Rigidbody _rigid;
    #endregion

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _statScript = GetComponent<Stats>();
        _playerHealth = GetComponent<Health>();
        _rigid = GetComponent<Rigidbody>();

        _agent.enabled = false;
        _agent.enabled = true;
    }

    private void OnEnable()
    {
        // �ǻ�Ƴ����� null
        targetedEnemy = null;
    }

    private void Start()
    {
        //PhotonView photonView = PhotonView.Get(this);
        //photonView.RPC("RPCStorageCaller", RpcTarget.MasterClient, playerStorage._id, playerStorage.session_id, playerStorage.userName, playerStorage.playerNumber, playerStorage.zera, playerStorage.ace, playerStorage.bet_id);

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                gameObject.tag = "Blue";
                EnemyTag = "Red";
            }

            else
            {
                gameObject.tag = "Red";
                //gameObject.layer = 8;
                EnemyTag = "Blue";
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                gameObject.tag = "Red";
                EnemyTag = "Blue";
            }

            else
            {
                gameObject.tag = "Blue";
                //gameObject.layer = 8;

                EnemyTag = "Red";
            }
        }
    }



    private void Update()
    {
        // �÷��̾� ��ġ���� ī�޶�� ����
        if (photonView.IsMine)
        {
            //_rigid.velocity = Vector3.zero;
            _rigid.angularVelocity = Vector3.zero;

            CurrentPlayerPos = transform.position;
            _agent.speed = _statScript.MoveSpeed;
            // sŰ ������ ����
            if (Input.GetKeyDown(KeyCode.S))
            {
                _agent.SetDestination(CurrentPlayerPos);
                _agent.stoppingDistance = 0f;
            }

            if (_playerHealth.isDeath == false)
            {
                MoveTo();
            }

        }
    }

    public Ray ray;
    public void MoveTo()
    {
        TargetRangeInterpolation();
        // a + ��Ŭ�� �̵�
        AutoTargetInput();
        ray = Cam.ScreenPointToRay(Input.mousePosition);

        // ��Ŭ���� �̵�
        if (Input.GetMouseButton(1))
        {
            // ��Ŭ���ϸ� ��Ŭ���̵� ����ġ�� �������
            inputA = false;

            // ���콺��ġ���� �� raycast�� ��ü�� �´´ٸ�, �װ��� navmesh��������
            if (Physics.Raycast(ray, out Hit, Mathf.Infinity))
            {
                MoveOntheGround(Hit);
                GetTargetedObject();

                // SMS Start --------------------------------------//
                moveMouseCanvas.transform.position = Hit.point;
                moveMouseObj.gameObject.SetActive(true);
                StartCoroutine(moveMousePointer.MoveMouseCursorPoint());
                // SMS End --------------------------------------//
            }
        }

        MoveEnemyPosition();
    }


    public void MoveOntheGround(RaycastHit hit)
    {
        // MOVEMENT
        // Ŭ���� ������ ������
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
            float dist = Vector3.Distance(transform.position, targetedEnemy.transform.position) - 0.5f - interpolationRange;

            // Ÿ���� ���ݹ��� ���϶�
            if (dist > _statScript.attackRange)
            {
                // �� ��ġ�� �̵��Ѵ�
                _agent.SetDestination(targetedEnemy.transform.position);
                _agent.stoppingDistance = _statScript.attackRange + interpolationRange;

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
                _agent.stoppingDistance = _statScript.attackRange + interpolationRange;

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
                    //photonView.RPC("IsAttack", RpcTarget.All);
                }
            }

        }
    }



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


    bool inputA = false;

    private void AutoTargetInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            inputA = true;
            // SMS Start ------------------------------------------------//
            // Ŀ���� ���� Ŀ���� �ٲ۴�.
            ChangeMouseAMode();
            // SMS End ---------------------------------------------------//
            Debug.Log($"inputA : {inputA}");
        }

        if (Input.GetMouseButtonDown(0) && inputA)
        {
            // SMS Start ------------------------------------------------//
            // Ŀ���� �Ϲ� Ŀ���� �ٲ۴�.
            Cursor.SetCursor(cursorTextureOriginal, hotSpot, cursorMode);
            // SMS End ---------------------------------------------------//

            inputA = false;
            // ���� ��ġ�� �̵��Ѵ�
            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out Hit, Mathf.Infinity))
            {
                MoveOntheGround(Hit);
            }
            AutoTargetMove();

        }


    }

    // SMS Start-------------------------------------------//
    public void ChangeMouseAMode()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    // SMS End-----------------------------------------------//



    float detectiveRange = 8f;
    private void AutoTargetMove()
    {

        // �±� �ް��ִ� ���ӿ�����Ʈ�� ã��
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);

        // �ͷ��� ���� ����� ��� �ӽ÷� ����
        //GameObject _shortTarget = null;

        //�ݶ��̴��� �ϳ��� ����Ǹ� ����
        if (enemies.Length > 0)
        {

            // ����� �ݶ��̴���ŭ �ݺ����ֱ�
            foreach (GameObject _colTarget in enemies)
            {

                // ������������ ���� ���´ٸ� 
                // �� ���� Ÿ�ٿ� �־���
                float distance = Vector3.Distance(transform.position, _colTarget.transform.position) - interpolationRange;
                if (distance <= detectiveRange)
                {
                    // �̴Ͼ�� �÷��̾ ���� ������ �÷��̾ �־��ش�
                    if (_colTarget.GetComponent<PlayerBehaviour>() != null)
                    {
                        GameObject enemyPlayer = _colTarget.GetComponent<PlayerBehaviour>().gameObject;
                        Debug.Log($"enemyPlayer : {enemyPlayer}");
                        targetedEnemy = enemyPlayer;
                    }
                    else
                    {
                        Debug.Log($"target : {_colTarget}");
                        targetedEnemy = _colTarget;
                    }

                }
            }
        }

        

    }

    float interpolationRange;
    private void TargetRangeInterpolation()
    {
        if (targetedEnemy == null)
        {
            return;
        }

        // �÷��̾� ����
        if (targetedEnemy.layer == 7)
        {
            interpolationRange = 0f;
        }
        // �̴Ͼ� ����
        else if (targetedEnemy.layer == 8)
        {
            interpolationRange = 0f;
        }
        // Ÿ�� ����
        else if (targetedEnemy.layer == 6)
        {
            interpolationRange = 2f;

        }
        // �ؼ��� ����
        else if (targetedEnemy.layer == 12)
        {
            interpolationRange = 7f;
        }
    }



}