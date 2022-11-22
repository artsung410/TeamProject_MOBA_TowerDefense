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

    #region DataParsing

    //private const string energyBoltURL = "https://docs.google.com/spreadsheets/d/1PnBV0AFMfz3PdaEXZJcOPjnQCCQCOGoV/export?format=tsv&range=A16:Y18";

    //IEnumerator GetSkillData(string url)
    //{
    //    UnityWebRequest GetSkillData = UnityWebRequest.Get(url);
    //    yield return GetSkillData.SendWebRequest();
    //    print(GetSkillData.downloadHandler.text);
    //}

    #endregion

    public GameObject DamazeZone;
    public float damageZoneRadius;
    public List<PlayerSkillDatas> Datas;

    Quaternion quaternion;
    float elasedTiem;
    Vector3 mouseDir;
    Vector3 currentPos; // 스킬 사용 위치

    private float HoldingTime;
    private float Damage;
    private float Range;
    private float Speed;
    private float lockTime;

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
        lockTime = SetLockTime;

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

    }

    public override void SkillUpdatePosition()
    {
        transform.Translate(Time.deltaTime * 30f * Vector3.forward);
        transform.rotation = quaternion;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            SkillUpdatePosition();
            elasedTiem += Time.deltaTime;
            if (elasedTiem >= lockTime)
            {
                _ability.OnLock(false);
            }

            float dist = Vector3.Distance(currentPos, transform.position);
            //Debug.Log($"날라간 거리 : {dist}");
            if (dist >= Range)
            {
                _ability.OnLock(false);
                PhotonNetwork.Destroy(gameObject);
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("충돌 확인용");
        if (photonView.IsMine)
        {
            if(other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                _ability.OnLock(false);
                SkillDamage(Damage, other.gameObject);
                PhotonNetwork.Destroy(gameObject);
            }

        }
    }

    private void OnDisable()
    {
        
    }
}
