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


    public GameObject damageZone;
    GameObject _obstacleDetector;

    #region Private 변수들

    Quaternion quaternion;
    float elapsedTime;
    Vector3 mouseDir;

    private float damage;
    private float speed;

    #endregion

    private void Awake()
    {
        _obstacleDetector = transform.GetChild(1).gameObject;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        damage = Data.Value_1;
        damageZone.GetComponent<SphereCollider>().radius = Data.RangeValue_1;

        damageZone.SetActive(false);
    }

    private void Start()
    {
        if (_ability == null)
        {
            return;
        }

        LookMouseCursor();
        CheckDist();

        _ability.OnLock(true);
        _ani.animator.SetBool("JumpAttack", true);

    }

    Vector3 leapPos;
    private void CheckDist()
    {
        Vector3 mousePos = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z);

        if (Vector3.Distance(_behaviour.transform.position, mousePos) >= Data.Range)
        {
            Vector3 startPos = _behaviour.transform.position;
            Vector3 endPos = _behaviour.transform.forward;
            leapPos = (startPos + endPos * Data.Range);
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

    bool isArrive;
    private void Update()
    {
        if (photonView.IsMine)
        {
            SkillUpdatePosition();

            // 지속시간동안 플레이어가 지정한 장소로 이동한다 => 도약은 애니메이션 처리
            _behaviour.transform.position = Vector3.MoveTowards(_behaviour.transform.position, leapPos, Time.deltaTime * 10f);

            // 원래 위치로 돌아가지 않도록 도착지를 최종목적지로 설정한다
            _behaviour.ForSkillAgent(leapPos);

            // 착지시 주변에 데미지를 준다(한번만 호출)
            //Debug.Log($"y축 차이가 얼마인가 : {_behaviour.transform.position.y}, {leapPos.y}");
            float diffY = Mathf.Abs(_behaviour.transform.position.y - leapPos.y);
            if (diffY >= 1f)
            {
                _behaviour.transform.position = new Vector3(_behaviour.transform.position.x, leapPos.y, _behaviour.transform.position.z);
            }

            if (Vector3.Distance(_behaviour.transform.position, leapPos) <= 1f)
            {
                //_damageZone.SetActive(true);
                photonView.RPC(nameof(RPC_Activate), RpcTarget.All);
                _ani.animator.SetBool("JumpAttack", false);
                isArrive = true;
            }

            if (isArrive)
            {
                SkillHoldingTime(Data.HoldingTime);
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

        if (elapsedTime >= Data.LockTime)
        {
            _ability.OnLock(false);
        }

        if (elapsedTime >= time)
        {
            if (photonView.IsMine)
            {
                photonView.RPC(nameof(RPC_Activate), RpcTarget.All);
                _ani.animator.SetBool("JumpAttack", false);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                SkillDamage(damage, other.gameObject);
            }
        }
    }

    // TODO : 충돌시 그자리에서 떨어지게 처리하였으나 좀더 나은 방법이 있는지 강구해볼것?
    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "Water" || collision.gameObject.tag == "Ground" || collision.gameObject.tag == _ability.tag && collision.gameObject.layer == 7)
            {
                return;
            }

            _behaviour.transform.forward = mouseDir;
            _behaviour.transform.position = transform.position;
            _behaviour.ForSkillAgent(transform.position);
            photonView.RPC(nameof(RPC_Activate), RpcTarget.All);
            _ani.animator.SetBool("JumpAttack", false);
            isArrive = true;
        }
    }

    // 착지한 지점에서 이펙트 동기화 문제(리모트캐릭터에선 이펙트 활성화 안되는중) => RPC로 해결
    [PunRPC]
    public void RPC_Activate()
    {
        Debug.Log("effect on");
        damageZone.SetActive(true);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
