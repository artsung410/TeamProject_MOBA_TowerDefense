using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject DamazeZone;

    public float damageZoneRadius;

    Quaternion quaternion;
    float elasedTiem;
    Vector3 mouseDir;
    Vector3 currentPos; // 스킬 사용 위치

    private float holdingTime;
    private float damage;
    private float range;
    private float Speed;
    private float lockTime;

    public float CrowdControlTime;

    private void Awake()
    {
        damageZoneRadius = DamazeZone.GetComponent<SphereCollider>().radius;
    }

    private void OnEnable()
    {
        elasedTiem = 0f;

        damage = SetDamage;
        range = SetRange;
        holdingTime = SetHodingTime;
        CrowdControlTime = 2f;
        lockTime = SetLockTime;

        damageZoneRadius = 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            RaycastHit hit;
            if (Physics.Raycast(_behaviour.ray, out hit))
            {
                mouseDir = new Vector3(hit.point.x, _behaviour.transform.position.y, hit.point.z) - _behaviour.transform.position;

                _behaviour.transform.forward = mouseDir;
                quaternion = _behaviour.transform.localRotation;
            }

            Vector3 mousePos = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z);
            if (Vector3.Distance(_behaviour.transform.position, mousePos) >= range)
            {
                Vector3 startPos = _behaviour.transform.position;
                Vector3 endPos = _behaviour.transform.forward;

                skillPos = startPos + endPos * range;
            }
            else
            {
                skillPos = mousePos;
            }

            _ability.OnLock(true);
        }
        catch (System.Exception)
        {
            //throw new System.Exception($"무슨일이야");
            print("리모트 스킬이라 null이 뜨는것이란다");
        }
    }

    Vector3 skillPos;

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        SkillUpdatePosition();
        SkillHoldingTime(holdingTime);
    }
    public override void SkillHoldingTime(float time)
    {
        elasedTiem += Time.deltaTime;

        if (elasedTiem >= lockTime)
        {
            _ability.OnLock(false);
        }

        if (elasedTiem >= time)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public override void SkillUpdatePosition()
    {
        transform.position = new Vector3(skillPos.x, 0, skillPos.z);
    }

    // TODO : 맞은 대상 스턴(2초동안)

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"충돌 테스트 : {other.gameObject.name}");

        if (photonView.IsMine)
        {
            // 중립 몬스터 : 태그없음, layer 17
            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                SkillDamage(damage, other.gameObject);
                CrowdControlStun(other.gameObject, CrowdControlTime, true);
            }
            
        }
    }

}
