using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Smash : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject damageZone;

    #region private 변수모음

    Quaternion quaternion;
    float elapsedTime;
    Vector3 mouseDir;

    private float damage;
    private float angle;
    #endregion

    private void Awake()
    {
        damageZone = GetComponentInChildren<Collider>().gameObject;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        damage = Data.Value_1;
        damageZone.GetComponent<SphereCollider>().radius = Data.Value_1;
        angle = Data.RangeValue_2 * 2;
    }

    void Start()
    {
        if (_ability == null)
        {
            return;
        }

        LookMouseCursor();
        _ability.OnLock(true);
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

    private void Update()
    {
        if (_ability == null)
        {
            return;
        }

        if (photonView.IsMine)
        {
            SkillUpdatePosition();
            SkillHoldingTime(Data.HoldingTime);
        }
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // 지속시간동안 플레이어가 공격하지 못한다
        _behaviour.targetedEnemy = null;
        _behaviour.perfomMeleeAttack = false;

        _ability.transform.rotation = quaternion;

        // 스킬을 쓰면 플레이어는 그자리에서 멈춘다
        _behaviour.ForSkillAgent(_behaviour.transform.position);

        if (elapsedTime >= Data.LockTime)
        {
            _ability.OnLock(false);
        }

        if (elapsedTime >= time)
        {
            
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public override void SkillUpdatePosition()
    {
        transform.position = _behaviour.transform.position;
        transform.rotation = quaternion;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                SectorDamage(other);
            }
        }
    }

    private void SectorDamage(Collider other)
    {
        Vector3 interV = other.gameObject.transform.position - _behaviour.gameObject.transform.position;

        if (interV.magnitude <= Data.Range)
        {
            float dot = Vector3.Dot(interV.normalized, _behaviour.gameObject.transform.forward);
            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            if (degree <= angle)
            {
                SkillDamage(damage, other.gameObject);
                SkillTimeDamage(Data.TickDamage, Data.TickTime, other.gameObject);
            }
        }
    }

}
