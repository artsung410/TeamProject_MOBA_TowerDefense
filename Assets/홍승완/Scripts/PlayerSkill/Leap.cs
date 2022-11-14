using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Leap : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################


    GameObject _damageZone;
    GameObject _obstacleDetector;

    #region Private 변수들

    Quaternion quaternion;
    float elapsedTime;
    string enemyTag;
    Vector3 mouseDir;

    private float HoldingTime;
    private float Damage;
    private float Range;
    private float Speed;
    #endregion

    private void Awake()
    {
        _damageZone = GetComponentInChildren<SphereCollider>().gameObject;
        _obstacleDetector = transform.GetChild(1).gameObject;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        Damage = SetDamage;
        HoldingTime = SetHodingTime;
        Range = SetRange;

        //isArive = false;
        //isAttack = false;

        _damageZone.SetActive(false);
    }

    private void Start()
    {
        if (_ability == null)
        {
            return;
        }

        TagProcessing(_ability);
        LookMouseCursor();

        CheckDist();

        _ani.animator.SetBool("JumpAttack", true);

    }

    Vector3 startPos;
    Vector3 endPos;
    Vector3 mousePos;
    Vector3 leapPos;
    private void CheckDist()
    {
        mousePos = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z);

        if (Vector3.Distance(_behaviour.transform.position, mousePos) >= Range)
        {
            startPos = _behaviour.transform.position;
            endPos = _behaviour.transform.forward;
            leapPos = (startPos + endPos * Range);
        }
        else
        {
            leapPos = mousePos;
        }

    }

    RaycastHit hit;
    public void LookMouseCursor()
    {
        // 마우스 방향에서 사용
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z) - _ability.transform.position;

            _ability.transform.forward = mouseDir;
            quaternion = _ability.transform.localRotation;
        }
    }
    private void TagProcessing(HeroAbility ability)
    {

        if (ability.CompareTag("Blue"))
        {
            enemyTag = "Red";
        }
        else if (ability.CompareTag("Red"))
        {
            enemyTag = "Blue";
        }
    }

    bool isArive;
    private void Update()
    {
        if (photonView.IsMine)
        {
            SkillUpdatePosition();

            // 지속시간동안 플레이어가 지정한 장소로 이동한다 => 도약은 애니메이션 처리
            _behaviour.transform.position = Vector3.Slerp(_behaviour.transform.position, leapPos, Time.deltaTime * 2.5f);

            // 원래 위치로 돌아가지 않도록 도착지를 최종목적지로 설정한다
            _behaviour.ForSkillAgent(leapPos);

            // 착지시 주변에 데미지를 준다(한번만 호출)
            if (Vector3.Distance(_behaviour.transform.position, leapPos) <= 0.1f)
            {
                //_damageZone.SetActive(true);
                photonView.RPC(nameof(RPC_Activate), RpcTarget.All);
                _ani.animator.SetBool("JumpAttack", false);
                isArive = true;
            }

            if (isArive)
            {
                SkillHoldingTime(HoldingTime);
            }
        }

    }


    public override void SkillUpdatePosition()
    {
        this.transform.position = _behaviour.transform.position;
        transform.forward = _behaviour.transform.forward;
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= time)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.CompareTag(enemyTag))
            {
                SkillDamage(Damage, other.gameObject);
            }
        }
    }

    // TODO : 충돌시 그자리에서 떨어지게 처리하였으나 좀더 나은 방법이 있는지 강구해볼것?
    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            // 충돌하면 
            // 바닥에 떨어짐(현재위치)

            // 플레이어 자신이 감지되고있어서 예외처리해줌 ㅠ
            if (collision.gameObject.tag == _ability.tag && collision.gameObject.layer == 7)
            {
                return;
            }

            isArive = true;
            _behaviour.transform.position = transform.position;
            photonView.RPC(nameof(RPC_Activate), RpcTarget.All);
            _ani.animator.SetBool("JumpAttack", false);
            _behaviour.ForSkillAgent(transform.position);
        }
    }

    // 착지한 지점에서 이펙트 동기화 문제(리모트캐릭터에선 이펙트 활성화 안되는중) => RPC로 해결
    [PunRPC]
    public void RPC_Activate()
    {
        _damageZone.SetActive(true);
    }
}
