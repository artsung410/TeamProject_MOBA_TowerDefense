using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChainAttack : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject[] Effect;

    public PlayerSkillDatas data;

    #region private 변수모음

    Quaternion quaternion;
    float elapsedTime;
    Vector3 mouseDir;

    private float holdingTime;
    private float damage;
    private float range;
    private float lockTime;

    #endregion


    private void Awake()
    {
        //effect = gameObject.GetComponent<ParticleSystem>();
        for (int i = 0; i < Effect.Length; i++)
        {
            Effect[i].SetActive(false);
        }

    }

    List<int> randomNum = new List<int>();    
    private void OnEnable()
    {
        elapsedTime = 0f;
        damage = SetDamage;
        //Debug.Log($"SetDamage : {SetDamage}");
        holdingTime = SetHodingTime;
        //Debug.Log($"SetHodingTime : {SetHodingTime}");
        range = SetRange;
        //Debug.Log($"SetRange : {SetRange}");
        lockTime = SetLockTime;
        //Debug.Log($"lockTime : {lockTime}");

        int currentNumber = Random.Range(0, 4);

        for (int i = 0; i < 4;)
        {
            if (randomNum.Contains(currentNumber))
            {
                currentNumber = Random.Range(0, 4);
            }
            else
            {
                randomNum.Add(currentNumber);
                i++;
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (_ability == null)
        {
            return;
        }

        //photonView.RPC(nameof(TagProcessing), RpcTarget.All);

        _ability.OnLock(true);
        LookMouseCursor();
    }

    public void LookMouseCursor()
    {
        // 마우스 방향에서 사용
        RaycastHit hit;
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z) - _ability.transform.position;

            _ability.transform.forward = mouseDir;
            // 스킬쓸때 플레이어 위치를 그곳으로 고정시키기 위해사용
            quaternion = _ability.transform.localRotation;
        }

    }

    float dispersionTime = 0f;
    bool isDamage = true;
    void Update()
    {
        if (_ability == null)
        {
            return;
        }

        if (_ability.gameObject.GetComponent<Health>().isDeath == true)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        // 랜덤한 이펙트 4개 켜주는 부분(아주 엉망진창임 그냥)
        if (testInt <= 3)
        {
            EnableTime += Time.deltaTime;
            if (EnableTime >= 0.1f)
            {
                photonView.RPC(nameof(RPC_RandomVFX), RpcTarget.All, testInt);
                testInt++;
                EnableTime = 0f;
            }
        }

        SkillUpdatePosition();
        SkillHoldingTime(holdingTime);

        dispersionTime += Time.deltaTime;
        
        float tickTime = holdingTime / 10;
        if (dispersionTime >= tickTime)
        {
            dispersionTime = 0f;
            isDamage = true;
        }

    }
    float EnableTime;
    int testInt;

    [PunRPC]
    public void RPC_RandomVFX(int idx)
    {
        Effect[randomNum[idx]].SetActive(true);
    }

    /// <summary>
    /// 스킬이 플레이어를 따라오게해줌
    /// </summary>
    public override void SkillUpdatePosition()
    {
        Debug.Log("위치 업데이트중");
        transform.position = _ability.transform.position;
        transform.rotation = quaternion;
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // Lock타임 동안 다른 스킬은 못쓰게 해준다
        if (elapsedTime >= lockTime)
        {
            _ability.OnLock(false);
        }

        // 지속시간동안 플레이어가 느려진다
        _stat.MoveSpeed = 3f;

        // 지속시간동안 플레이어가 공격하지 못한다
        _behaviour.targetedEnemy = null;
        _behaviour.perfomMeleeAttack = false;

        // 지속시간동안 플레이어는 스킬방향만 바라본다
        _ability.transform.rotation = quaternion;

        // 지속시간이 끝나면 사라진다
        if (elapsedTime >= time)
        {
            _stat.MoveSpeed = 15f;
            // Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left 오류발생
            // 해결법 : photonView.IsMine 조건 추가
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 데미지 두번들어가던부분 IsMine으로 처리
        if (photonView.IsMine)
        {
            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                if (isDamage)
                {
                    isDamage = false;
                    float tickDamage = damage / 10;
                    SkillDamage(tickDamage, other.gameObject);
                }
            }
        }
    }
}
