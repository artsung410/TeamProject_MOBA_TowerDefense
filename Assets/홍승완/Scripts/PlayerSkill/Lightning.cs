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

            throw new System.Exception($"무슨일이야");
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
            Destroy(gameObject);
            //PhotonNetwork.Destroy(gameObject);
        }
    }

    public override void SkillUpdatePosition()
    {
        transform.position = skillPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"충돌 테스트 : {other.gameObject.name}");

        if (photonView.IsMine)
        {
            if (other.CompareTag(enemyTag))
            {
                SkillDamage(Damage, other.gameObject);
            }
        }
    }
}
