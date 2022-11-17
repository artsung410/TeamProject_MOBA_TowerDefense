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
    string enemyTag;
    Vector3 mouseDir;
    Vector3 currentPos; // 스킬 사용 위치

    private float HoldingTime;
    private float Damage;
    private float Range;
    private float Speed;

    public float CrowdControlTime;

    private void Awake()
    {
        damageZoneRadius = DamazeZone.GetComponent<SphereCollider>().radius;
    }

    private void OnEnable()
    {
        elasedTiem = 0f;

        Damage = SetDamage;
        Range = SetRange;
        HoldingTime = SetHodingTime;
        CrowdControlTime = 2f;

        damageZoneRadius = 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            if (_ability.CompareTag("Blue"))
            {
                enemyTag = "Red";
            }
            else if (_ability.CompareTag("Red"))
            {
                enemyTag = "Blue";
            }

            RaycastHit hit;
            if (Physics.Raycast(_behaviour.ray, out hit))
            {
                mouseDir = new Vector3(hit.point.x, _behaviour.transform.position.y, hit.point.z) - _behaviour.transform.position;

                _behaviour.transform.forward = mouseDir;
                quaternion = _behaviour.transform.localRotation;
            }

            Vector3 mousePos = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z);
            if (Vector3.Distance(_behaviour.transform.position, mousePos) >= Range)
            {
                Vector3 startPos = _behaviour.transform.position;
                Vector3 endPos = _behaviour.transform.forward;

                skillPos = startPos + endPos * Range;
            }
            else
            {
                skillPos = mousePos;
            }
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
        SkillHoldingTime(HoldingTime);
    }
    public override void SkillHoldingTime(float time)
    {
        elasedTiem += Time.deltaTime;

        if (elasedTiem >= time)
        {
            //Destroy(gameObject);
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
            if (other.CompareTag(enemyTag) || other.gameObject.layer == 17)
            {
                SkillDamage(Damage, other.gameObject);
                CrowdControlStun(other.gameObject, CrowdControlTime, true);
            }
            
        }
    }

    // 스킬이 isStun을 true로 만들어주는데 몇초동안 유지할지 까지 알려준다
}
