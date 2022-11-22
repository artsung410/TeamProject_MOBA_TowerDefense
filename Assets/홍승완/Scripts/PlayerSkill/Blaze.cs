//#define VER_Y_0
#define VER_Y_PLAYER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Blaze : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject DamageZone;

    Quaternion quaternion;
    float elapsedTime;
    Vector3 mouseDir;

    private float holdingTime;
    private float damage;
    private float range;
    private float speed;
    private float lockTime;

    float width;
    float length;
    float zCenter;

    private void Awake()
    {
        width = DamageZone.GetComponent<BoxCollider>().size.x;
        length = DamageZone.GetComponent<BoxCollider>().size.z;
        zCenter = DamageZone.GetComponent<BoxCollider>().center.z;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        damage = SetDamage;
        holdingTime = SetHodingTime;
        range = SetRange;
        lockTime = SetLockTime;

        width = range;
        length = 3;

        zCenter = width / 2f;
    }

    void Start()
    {
        try
        {
            LookMouseDir();
            _ability.OnLock(true);
        }
        catch (System.Exception)
        {
            print("리모트 스킬 null참조중");
        }
    }

    private void LookMouseDir()
    {
        RaycastHit hit;
#if VER_Y_PLAYER
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _behaviour.transform.position.y, hit.point.z) - _behaviour.transform.position;

            _behaviour.transform.forward = mouseDir;
            quaternion = _behaviour.transform.localRotation;
        }
#endif

#if VER_Y_0
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, 0, hit.point.z) - _behaviour.transform.position;

            _behaviour.transform.forward = mouseDir;
            quaternion = _behaviour.transform.localRotation;
        }
#endif
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            SkillUpdatePosition();
            SkillHoldingTime(holdingTime);
        }
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= lockTime)
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
        transform.rotation = quaternion;
    }

    // TODO : 지속 데미지 추가할것
    /*
    블레이즈 스킬 정의
    플레이어 앞으로 생성된다
    사이즈는 12 * 3 사각형이다
    지속시간(2초)동안 바닥에 깔려있다
    지속피해가 존재한다
        5초동안 5틱 -> 1초에 1틱 30데미지
        지속피해는 한번만 들어간다
     */

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                SkillTimeDamage(30, 5f, other.gameObject);
                SkillDamage(damage, other.gameObject);
            }
        }
    }
}
