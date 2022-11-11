using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class PlayerBehaviour : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com
    //             MAIL : minsub4400@gmail.com   
    // ###############################################

    public static Vector3 CurrentPlayerPos;

    [Header("ȸ�� �ӵ�")]
    [SerializeField] float RotateSpeed = 0.5f;
    public float RotateVelocity;

    [Header("���̾��")]
    public LayerMask Layer;
    RaycastHit Hit;

    // ���� ���� ī�޶�
    public Camera Cam;

    [Header("Ÿ��")]
    public GameObject targetedEnemy;

    [Header("�������ΰ�")]
    public bool perfomMeleeAttack = false;
    public bool perfomRangeAttack = false;

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

    public Collider enemyCol;



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
        //StartCoroutine(DetectEnemyRange());
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

        //photonView.RPC(nameof(RPC_ApplyTargetNull), RpcTarget.All);
    }

    // TODO : ���� ĳ���͸� ����ؼ� �����Ѵ�.

    /// <summary>
    /// �ֺ��� ���� �÷��̾� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator DetectEnemyRange()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }


    private void Update()
    {
        // ���� ������ ����
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

        // �÷��̾� ��ġ���� ī�޶�� ����
        if (photonView.IsMine)
        {
            _rigid.velocity = Vector3.zero;
            _rigid.angularVelocity = Vector3.zero;

            CurrentPlayerPos = transform.position;
            _agent.speed = _statScript.MoveSpeed;
            // sŰ ������ �ൿ ����
            if (Input.GetKeyDown(KeyCode.S))
            {
                _agent.SetDestination(CurrentPlayerPos);
                _agent.stoppingDistance = 0f;

                CancelInvoke(nameof(AutoSearchTarget));
            }

            if (_playerHealth.isDeath == false)
            {
                MoveTo();
            }
            else
            {
                IsPlayerDie();
            }
        }
    }

    public void ForLeapFuction(PlayerBehaviour player, float jumpForce, float fallMultiplier)
    {
        player._rigid.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

        if (player._rigid.velocity.y < 0)
        {
            player._rigid.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }
    }

    private void IsPlayerDie()
    {
        // ���ڸ����� ����
        _agent.SetDestination(CurrentPlayerPos);
        _agent.stoppingDistance = 0f;

        // �� ã�� ����
        CancelInvoke(nameof(AutoSearchTarget));
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
            CancelInvoke(nameof(AutoSearchTarget));

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
            // ��Ÿ� ������ ���ö�
            else
            {
                // �������� �ʰ� ���� ��ġ�� ����
                //_agent.SetDestination(transform.position);
                _agent.stoppingDistance = _statScript.attackRange;

                // Ÿ���� �ٶ󺻴�
                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref RotateVelocity,
                    RotateSpeed * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);

                // ���� ����ĳ���
                if (_statScript.AttackType == HeroAttackType.Melee)
                {
                    // ���� ���� ����ġ�� true�� �ٲ�
                    perfomMeleeAttack = true;
                }
                else if (_statScript.AttackType == HeroAttackType.Ranged)
                {
                    // ���Ÿ� ���� ���� ����ġ true�ٲ�
                    perfomRangeAttack = true;
                }
            }

        }
    }

    // TODO : ���� Ÿ�Կ� ���� ���ݹ�� ����

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

            InvokeRepeating(nameof(AutoSearchTarget), 0, 0.5f);
        }
    }

    // SMS Start-------------------------------------------//
    public void ChangeMouseAMode()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    // SMS End-----------------------------------------------//

    float detectiveRange;

    private void AutoSearchTarget()
    {
        detectiveRange = _statScript.attackRange;
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, detectiveRange);
        GameObject shortTarget = null;
        if (colliders.Length > 0)
        {
            // ª���Ÿ��� ���Ϸ��� ���� �� ��ü�� ����
            // ���Ѵ� ���� ����
            float _shortTempDistance = Mathf.Infinity; 
            foreach (Collider colTarget in colliders)
            {
                // ������ �� �÷��̾�� ������ 1���� ����
                if (colTarget.tag == EnemyTag && colTarget.gameObject.layer == 7)
                {
                    targetedEnemy = colTarget.gameObject;
                    return;
                }
                // �׿�(�̴Ͼ�, Ÿ��, �ؼ���)�Ÿ��� ����
                else if (colTarget.tag == EnemyTag)
                {
                    float colDist = Vector3.Distance(this.transform.position, colTarget.gameObject.transform.position);

                    if (_shortTempDistance > colDist)
                    {
                        _shortTempDistance = colDist;

                        // ���� ����� ���
                        shortTarget = colTarget.gameObject;
                    }
                }
                // ���ܺκ�(��, ��Ÿ���) �ǳʶٱ�
                else
                {
                    continue;
                }
            }

        }
        else
        {
            // �ֺ��� ���̾����� null
            targetedEnemy = null;
        }

        if (shortTarget != null)
        {
            // �ݺ��� �Ϸ�� ���� ����� ��� ����
            targetedEnemy = shortTarget.gameObject;
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
            interpolationRange = 1f;
        }
        // �̴Ͼ� ����
        else if (targetedEnemy.layer == 8)
        {
            interpolationRange = 0f;
        }
        // Ÿ�� ����
        else if (targetedEnemy.layer == 6)
        {
            interpolationRange = 1f;
        }
        // �ؼ��� ����
        else if (targetedEnemy.layer == 12)
        {
            interpolationRange = 6f;
        }
    }

    public void ForSkillAgent(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }


    // SetActive(false)�Ǹ� �����
    private void OnDisable()
    {
        StopAllCoroutines();

    }

}