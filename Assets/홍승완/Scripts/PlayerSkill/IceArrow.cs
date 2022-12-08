using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IceArrow : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject DamageZone;

    Quaternion quaternion;
    float elasedTiem;
    Vector3 mouseDir;
    Vector3 currentPos; // 스킬 사용 위치

    private float damage;
    private float width;
    private float vertical;
    private bool isHit;

    private void Awake()
    {
        width = DamageZone.GetComponent<BoxCollider>().size.z;
        vertical = DamageZone.GetComponent<BoxCollider>().size.x;
    }

    private void OnEnable()
    {
        elasedTiem = 0f;

        damage = Data.Value_1;
        width = Data.RangeValue_1;
        vertical = Data.RangeValue_2;

        currentPos = transform.position;

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
        catch (System.Exception ie)
        {
            print(ie);
        }
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

    public override void SkillHoldingTime(float time)
    {
        throw new System.NotImplementedException();
    }

    public override void SkillUpdatePosition()
    {
        transform.Translate(Time.deltaTime * speed * Vector3.forward);
        transform.rotation = quaternion;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.CompareTag(_behaviour.tag))
            {
                return;
            }

            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                BuffBlueprint buff = CSVtest.Instance.BuffDic[Data.ID + 100]; // 이동속도 감소
                if (other.gameObject.layer == 7 || other.CompareTag(enemyTag))
                {
                    BuffManager.Instance.AddBuff(buff);
                }

                SkillDamage(damage, other.gameObject);

                _ability.OnLock(false);
                PhotonNetwork.Destroy(gameObject);
            }
            return;
        }
    }
}
