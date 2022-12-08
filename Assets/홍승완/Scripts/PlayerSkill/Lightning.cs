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

    public GameObject DamageZone;

    Quaternion quaternion;
    float elasedTiem;
    Vector3 mouseDir;

    private float damage;
    private float crowdControlTime;

    private void Awake()
    {
        DamageZone = GetComponentInChildren<Collider>().gameObject;
        DamageZone.GetComponent<SphereCollider>().radius = Data.RangeValue_1;
    }

    private void OnEnable()
    {
        elasedTiem = 0f;

        damage = Data.Value_1;
        crowdControlTime = Data.CcTime;

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
            if (Vector3.Distance(_behaviour.transform.position, mousePos) >= Data.Range)
            {
                Vector3 startPos = _behaviour.transform.position;
                Vector3 endPos = _behaviour.transform.forward;

                skillPos = startPos + endPos * Data.Range;
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
        SkillHoldingTime(Data.HoldingTime);
    }
    public override void SkillHoldingTime(float time)
    {
        elasedTiem += Time.deltaTime;

        if (elasedTiem >= Data.LockTime)
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
        transform.position = skillPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.CompareTag(_behaviour.tag))
            {
                return;
            }

            // 중립 몬스터 : 태그없음, layer 17
            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                //CrowdControlStun(other.gameObject, crowdControlTime, true);
                BuffBlueprint buff = CSVtest.Instance.BuffDic[Data.ID + 100]; // 기절
                if (other.gameObject.layer == 7 || other.CompareTag(enemyTag))
                {
                    BuffManager.Instance.AddBuff(buff);
                }
                SkillDamage(damage, other.gameObject);
            }
            
        }
    }

}
