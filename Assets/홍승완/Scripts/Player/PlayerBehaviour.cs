using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public bool isStun; // ���ϻ��½� -> true
    public float StunTime;
    
    [Space]
    
    public Collider enemyCol;
    // SMS Start --------------------------------------------//

    // �̵� �Ϲ� Ŀ��
    public Texture2D cursorMoveNamal;
    // �̵� �Ʊ� Ŀ��
    public Texture2D cursorMoveAlly;
    // �̵� ���� Ŀ��
    public Texture2D cursorMoveEnemy;

    // ���� �Ϲ� Ŀ��
    public Texture2D cusorAttackNomal;
    // ���� �Ʊ� Ŀ��
    public Texture2D cusorAttackAlly;
    // ���� ���� Ŀ��
    public Texture2D cusorAttackEnemy;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public Canvas moveMouseCanvas;
    public GameObject moveMouseObj1;
    public GameObject moveMouseObj2;
    public MousePointer moveMousePointer;
    // SMS End --------------------------------------------//

    #region Other Components
    public PlayerAnimation motion;
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

        Cursor.SetCursor(cursorMoveNamal, hotSpot, cursorMode);

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

    }

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

    float recoveryTime;
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
            _agent.speed = _statScript.moveSpeed;
            // sŰ ������ �ൿ ����
            if (Input.GetKeyDown(KeyCode.S) || isStun)
            {
                targetedEnemy = null;
                _agent.SetDestination(CurrentPlayerPos);
                _agent.stoppingDistance = 0f;
                CancelInvoke(nameof(AutoSearchTarget));
                return;
            }
            else
            {
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
    }

    public void OnStun(bool stun)
    {
        if (_playerHealth.isDeath)
        {
            return;
        }
        photonView.RPC(nameof(RPC_Stun), RpcTarget.All, stun);
    }

    [PunRPC]
    public void RPC_Stun(bool stun)
    {
        isStun = stun;
        motion.DizzyMotion(stun);
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
                if (isAKey == false)
                    moveMouseObj2.gameObject.SetActive(false);

                moveMouseCanvas.transform.position = Hit.point;
                moveMouseObj1.gameObject.SetActive(true);
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
            float dist = Vector3.Distance(transform.position, targetedEnemy.transform.position) - interpolationRange;
            //Debug.Log($"Ÿ�ٰ��� �Ÿ� : {dist}");
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
            // ��Ÿ� ������ ���ö�
            else
            {
                // �������� �ʰ� ���� ��ġ�� ����
                _agent.SetDestination(transform.position);
                _agent.stoppingDistance = dist;

                // Ÿ���� �ٶ󺻴�
                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref RotateVelocity,
                    RotateSpeed * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);

                // ���� ����ĳ���
                if (_statScript.AttackType == HeroType.Warrior)
                {
                    // ���� ���� ����ġ�� true�� �ٲ�
                    perfomMeleeAttack = true;
                }
                else if (_statScript.AttackType == HeroType.Wizard)
                {
                    // ���Ÿ� ���� ���� ����ġ true�ٲ�
                    perfomRangeAttack = true;
                }
            }

        }
        else
        {
            if (_statScript.AttackType == HeroType.Warrior)
            {
                perfomMeleeAttack = false;
            }
            else if (_statScript.AttackType == HeroType.Wizard)
            {
                perfomRangeAttack = false;
            }
        }
    }

    private void GetTargetedObject()
    {
        if (Hit.collider.CompareTag(EnemyTag) || Hit.collider.gameObject.layer == 17)
        {
            targetedEnemy = Hit.collider.gameObject;
        }
        else
        {
            targetedEnemy = null;
        }
    }

    bool isAKey = false;
    bool inputA = false;
    private void AutoTargetInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            inputA = true;
            // SMS Start ------------------------------------------------//
            // Ŀ���� ���� Ŀ���� �ٲ۴�.
            ChangeMouseAMode();
            // A�� ������
            isAKey = true;
            // SMS End ---------------------------------------------------//
        }

        if (Input.GetMouseButtonDown(0) && inputA)
        {
            // SMS Start ------------------------------------------------//
            // Ŀ���� �Ϲ� Ŀ���� �ٲ۴�.
            Cursor.SetCursor(cursorMoveNamal, hotSpot, cursorMode);
            // AŰ�� ������ ���� Ŭ���� �Ǿ��ٸ� ����

            
            inputA = false;
            // ���� ��ġ�� �̵��Ѵ�
            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out Hit, Mathf.Infinity))
            {
                if (isAKey)
                {
                    moveMouseObj1.gameObject.SetActive(false);
                    moveMouseCanvas.transform.position = Hit.point;
                    moveMouseObj2.gameObject.SetActive(true);
                    StartCoroutine(moveMousePointer.MoveMouseCursorPoint());
                    isAKey = false;
                }

                if (Hit.collider.CompareTag(EnemyTag) || Hit.collider.gameObject.layer == 17)
                {
                    targetedEnemy = Hit.collider.gameObject;
                }

                if (targetedEnemy != null)
                {
                    return;
                }

                MoveOntheGround(Hit);
                GetTargetedObject();

            }
            // SMS End ---------------------------------------------------//
            
            InvokeRepeating(nameof(AutoSearchTarget), 0, 0.5f);
        }
    }

    // SMS Start-------------------------------------------//
    public void ChangeMouseAMode()
    {
        Cursor.SetCursor(cusorAttackNomal, hotSpot, cursorMode);
    }
    // SMS End-----------------------------------------------//

    float detectiveRange;

    private void AutoSearchTarget()
    {
        Debug.Log("AutoSearch ������");
        if (targetedEnemy != null)
        {
            return;
        }
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
                // �߸� ���� ����
                else if (colTarget.gameObject.layer == 17)
                {
                    targetedEnemy = colTarget.gameObject;
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
            interpolationRange = 0.5f;
        }
        // �̴Ͼ� ����
        else if (targetedEnemy.layer == 8)
        {
            interpolationRange = 0f;
        }
        // Ÿ�� ����
        else if (targetedEnemy.layer == 6)
        {
            interpolationRange = 3.5f;
        }
        // �ؼ��� ����
        else if (targetedEnemy.layer == 12)
        {
            interpolationRange = 7f;
        }
        // �߸����� ����
        else if (targetedEnemy.layer == 17)
        {
            interpolationRange = 2f;
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