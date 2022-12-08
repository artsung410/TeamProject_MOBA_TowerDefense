using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Whirlwind : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject damageZone;

    #region Private 변수들

    Quaternion quaternion;
    float elapsedTime;
    //string enemyTag;
    Vector3 mouseDir;

    private float damage;
    private float speed;
    private float vertical;
    #endregion

    // TODO : 휠윈드 이펙트부분 수정필요

    private void Awake()
    {
        vertical = damageZone.GetComponent<BoxCollider>().size.z;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        damage = Data.Value_1;
        vertical = Data.Range;
        speed = 500f;
    }

    private void Start()
    {
        if (_ability == null)
        {
            return;
        }

        _ability.OnLock(true);

    }

    private void Update()
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

        if (photonView.IsMine)
        {

            SkillUpdatePosition();
            SkillHoldingTime(Data.HoldingTime);
        }
    }
    public override void SkillUpdatePosition()
    {
        // 스킬은 플레이어 주변에 있다
        transform.position = _ability.gameObject.transform.position;
        // 회전을 한다
        damageZone.transform.Rotate(speed * Time.deltaTime * Vector3.up);

        
        
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // TODO : 도는 모션은 애니메이션으로 처리할것

        // 지속시간동안 플레이어가 공격하지 못한다
        _behaviour.targetedEnemy = null;
        _behaviour.perfomMeleeAttack = false;

        if (elapsedTime >= Data.LockTime)
        {
            _ability.OnLock(false);
        }

        // 검기 지속시간이 끝나면 사라지게한다
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
            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                BuffBlueprint buff = CSVtest.Instance.BuffDic[Data.ID + 50];
                if (other.gameObject.layer == 7 && other.CompareTag(enemyTag))
                {
                    BuffManager.Instance.AddBuff(buff);
                }

                SkillDamage(damage, other.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
