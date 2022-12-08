using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.Networking;

public class EnergyBolt : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject DamazeZone;

    Quaternion quaternion;
    float elasedTiem;
    Vector3 mouseDir;
    Vector3 currentPos; // 스킬 사용 위치

    private float Damage;

    private void Awake()
    {
        DamazeZone.GetComponent<SphereCollider>().radius = Data.RangeValue_1;
    }

    private void OnEnable()
    {
        elasedTiem = 0;
        currentPos = transform.position;        

        Damage = Data.Value_1;

        speed = Data.Value_2;
        if (speed == 0)
        {
            speed = 20f;
        }
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

            _ability.OnLock(true);
        }
        catch (System.Exception ex)
        {
            //throw new Exception($"null 참조중{gameObject.name}");
            print(ex);
        }
    }

    public override void SkillHoldingTime(float time)
    {
        Debug.Log("투사체 속도 필요");
    }

    public override void SkillUpdatePosition()
    {
        transform.Translate(Time.deltaTime * speed * Vector3.forward);
        transform.rotation = quaternion;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            SkillUpdatePosition();
            elasedTiem += Time.deltaTime;
            if (elasedTiem >= Data.LockTime)
            {
                _ability.OnLock(false);
            }

            float dist = Vector3.Distance(currentPos, transform.position);
            if (dist >= Data.Range)
            {
                _ability.OnLock(false);
                PhotonNetwork.Destroy(gameObject);
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.CompareTag(_behaviour.tag))
            {
                return;
            }

            if(other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                _ability.OnLock(false);
                SkillDamage(Damage, other.gameObject);
                PhotonNetwork.Destroy(gameObject);
            }

        }
    }

}
