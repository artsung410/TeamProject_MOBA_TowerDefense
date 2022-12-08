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

    [Header("회전 속도")]
    [SerializeField] float RotateSpeed = 0.5f;
    public float RotateVelocity;

    [Header("레이어설정")]
    public LayerMask Layer;
    RaycastHit Hit;

    // 레이 찍을 카메라
    public Camera Cam;

    [Header("타겟")]
    public GameObject targetedEnemy;

    [Header("공격중인가")]
    public bool perfomMeleeAttack = false;
    public bool perfomRangeAttack = false;
    public string EnemyTag;
    public bool isStun; // 스턴상태시 -> true
    public float StunTime;
    
    [Space]
    
    public Collider enemyCol;
    // SMS Start --------------------------------------------//

    // 이동 일반 커서
    public Texture2D cursorMoveNamal;
    // 이동 아군 커서
    public Texture2D cursorMoveAlly;
    // 이동 적군 커서
    public Texture2D cursorMoveEnemy;

    // 공격 일반 커서
    public Texture2D cusorAttackNomal;
    // 공격 아군 커서
    public Texture2D cusorAttackAlly;
    // 공격 적군 커서
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
        // 되살아났을때 null
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
    /// 주변의 적군 플레이어 감지
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
        // 게임 끝나면 정지
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

        // 플레이어 위치정보 카메라로 보냄
        if (photonView.IsMine)
        {

            _rigid.velocity = Vector3.zero;
            _rigid.angularVelocity = Vector3.zero;

            CurrentPlayerPos = transform.position;
            _agent.speed = _statScript.moveSpeed;
            // s키 누르면 행동 멈춤
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
        // 그자리에서 죽음
        _agent.SetDestination(CurrentPlayerPos);
        _agent.stoppingDistance = 0f;

        // 적 찾기 멈춤
        CancelInvoke(nameof(AutoSearchTarget));
    }

    public Ray ray;
    public void MoveTo()
    {
        TargetRangeInterpolation();
        // a + 좌클릭 이동
        AutoTargetInput();
        ray = Cam.ScreenPointToRay(Input.mousePosition);

        // 우클릭시 이동
        if (Input.GetMouseButton(1))
        {
            // 우클릭하면 좌클릭이동 스위치를 취소해줌
            inputA = false;
            CancelInvoke(nameof(AutoSearchTarget));

            // 마우스위치에서 쏜 raycast가 물체에 맞는다면, 그곳이 navmesh도착지점
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
        // 클릭한 지점이 목적지
        _agent.SetDestination(hit.point);
        targetedEnemy = null;
        _agent.stoppingDistance = 0;

        // ROTATION
        Quaternion lookAtTheTarget = Quaternion.LookRotation(hit.point - transform.position);
        // 캐릭터가 타겟을 향해 회전하는 속도 보간
        // RotateSpeed값이 작을수록 빠르게 회전함
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtTheTarget.eulerAngles.y, ref RotateVelocity, RotateSpeed * (Time.deltaTime * 5));

        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }

    public void MoveEnemyPosition()
    {
        // 타겟이 있을때
        if (targetedEnemy != null)
        {
            float dist = Vector3.Distance(transform.position, targetedEnemy.transform.position) - interpolationRange;
            //Debug.Log($"타겟과의 거리 : {dist}");
            // 타겟이 공격범위 밖일때
            if (dist > _statScript.attackRange)
            {
                // 그 위치로 이동한다
                _agent.SetDestination(targetedEnemy.transform.position);
                _agent.stoppingDistance = _statScript.attackRange;

                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref RotateVelocity,
                    RotateSpeed * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);
            }
            // 사거리 안으로 들어올땐
            else
            {
                // 움직이지 않게 현재 위치로 고정
                _agent.SetDestination(transform.position);
                _agent.stoppingDistance = dist;

                // 타겟을 바라본다
                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref RotateVelocity,
                    RotateSpeed * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);

                // 내가 근접캐라면
                if (_statScript.AttackType == HeroType.Warrior)
                {
                    // 공격 수행 스위치를 true로 바꿈
                    perfomMeleeAttack = true;
                }
                else if (_statScript.AttackType == HeroType.Wizard)
                {
                    // 원거리 공격 수행 스위치 true바꿈
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
            // 커서를 공격 커서로 바꾼다.
            ChangeMouseAMode();
            // A를 눌렀음
            isAKey = true;
            // SMS End ---------------------------------------------------//
        }

        if (Input.GetMouseButtonDown(0) && inputA)
        {
            // SMS Start ------------------------------------------------//
            // 커서를 일반 커서로 바꾼다.
            Cursor.SetCursor(cursorMoveNamal, hotSpot, cursorMode);
            // A키가 눌리고 왼쪽 클릭이 되었다면 실행

            
            inputA = false;
            // 누른 위치로 이동한다
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
        Debug.Log("AutoSearch 실행중");
        if (targetedEnemy != null)
        {
            return;
        }
        detectiveRange = _statScript.attackRange;
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, detectiveRange);
        GameObject shortTarget = null;
        if (colliders.Length > 0)
        {
            // 짧은거리를 비교하려면 가장 긴 객체가 기준
            // 무한대 길이 생성
            float _shortTempDistance = Mathf.Infinity; 
            foreach (Collider colTarget in colliders)
            {
                // 범위내 적 플레이어는 무조건 1순위 검출
                if (colTarget.tag == EnemyTag && colTarget.gameObject.layer == 7)
                {
                    targetedEnemy = colTarget.gameObject;
                    return;
                }
                // 그외(미니언, 타워, 넥서스)거리순 검출
                else if (colTarget.tag == EnemyTag)
                {
                    float colDist = Vector3.Distance(this.transform.position, colTarget.gameObject.transform.position);

                    if (_shortTempDistance > colDist)
                    {
                        _shortTempDistance = colDist;

                        // 가장 가까운 대상
                        shortTarget = colTarget.gameObject;
                    }
                }
                // 중립 몬스터 검출
                else if (colTarget.gameObject.layer == 17)
                {
                    targetedEnemy = colTarget.gameObject;
                }
                // 예외부분(땅, 기타등등) 건너뛰기
                else
                {
                    continue;
                }
            }

        }
        else
        {
            // 주변에 적이없으면 null
            targetedEnemy = null;
        }

        if (shortTarget != null)
        {
            // 반복문 완료시 가장 가까운 대상 검출
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

        // 플레이어 보간
        if (targetedEnemy.layer == 7)
        {
            interpolationRange = 0.5f;
        }
        // 미니언 보간
        else if (targetedEnemy.layer == 8)
        {
            interpolationRange = 0f;
        }
        // 타워 보간
        else if (targetedEnemy.layer == 6)
        {
            interpolationRange = 3.5f;
        }
        // 넥서스 보간
        else if (targetedEnemy.layer == 12)
        {
            interpolationRange = 7f;
        }
        // 중립몬스터 보간
        else if (targetedEnemy.layer == 17)
        {
            interpolationRange = 2f;
        }
    }

    public void ForSkillAgent(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }


    // SetActive(false)되면 실행됨
    private void OnDisable()
    {
        StopAllCoroutines();

    }

}