using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class EnergyBolt : SkillHandler
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
    Vector3 currentPos; // ��ų ��� ��ġ

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
        elasedTiem = 0;

        Damage = SetDamage;
        HoldingTime = SetHodingTime;
        Range = SetRange;
        currentPos = transform.position;
        
        damageZoneRadius = 2;
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

            if (_ability.CompareTag("Blue"))
            {
                enemyTag = "Red";
            }
            else if (_ability.CompareTag("Red"))
            {
                enemyTag = "Blue";
            }
        }
        catch (System.Exception)
        {
            throw new Exception($"null ������{gameObject.name}");
        }
    }

    public override void SkillHoldingTime(float time)
    {

    }

    public override void SkillUpdatePosition()
    {
        transform.Translate(Time.deltaTime * 40f * Vector3.forward);
        transform.rotation = quaternion;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            SkillUpdatePosition();

            float dist = Vector3.Distance(currentPos, transform.position);
            //Debug.Log($"���� �Ÿ� : {dist}");
            if (dist >= Range)
            {
                Destroy(gameObject);
                //PhotonNetwork.Destroy(gameObject);
            }
            
        }
    }
    // TODO : ��Ÿ��� 10�� �ʹ� ª��!
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�浹 Ȯ�ο�");
        if (photonView.IsMine)
        {
            if (other.gameObject.tag != enemyTag)
            {
                return;
            }
            else
            {
                SkillDamage(Damage, other.gameObject);
                Destroy(gameObject);
                //PhotonNetwork.Destroy(gameObject);
            }

        }
    }

    private void OnDisable()
    {
        
    }
}
