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
    public float xSize; // 가로
    public float zSize; // 세로?

    Quaternion quaternion;
    float elasedTiem;
    string enemyTag;
    Vector3 mouseDir;
    Vector3 currentPos; // 스킬 사용 위치

    private float holdingTime;
    private float damage;
    private float range;
    private float speed;
    private float lockTime;

    private float CrowedControlTime;
    private float CrowedControlValue;

    public BuffData deBuff;

    private void Awake()
    {
        zSize = DamageZone.GetComponent<BoxCollider>().size.z;
        xSize = DamageZone.GetComponent<BoxCollider>().size.x;
    }

    private void OnEnable()
    {
        elasedTiem = 0f;
        zSize = 2f;
        xSize = 1f;

        damage = SetDamage;
        holdingTime = SetHodingTime;
        range = SetRange;
        lockTime = SetLockTime;

        currentPos = transform.position;

        CrowedControlTime = 3f;
        CrowedControlValue = 0.4f;
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
            if (elasedTiem >= lockTime)
            {
                _ability.OnLock(false);
            }

            float dist = Vector3.Distance(currentPos, transform.position);
            if (dist >= range)
            {
                _ability.OnLock(false);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    // 10을 날아가는 투사체에 HoldingTime이 의미가 있는건가?
    public override void SkillHoldingTime(float time)
    {
        throw new System.NotImplementedException();
    }

    public override void SkillUpdatePosition()
    {
        transform.Translate(Time.deltaTime * 10f * Vector3.forward);
        transform.rotation = quaternion;
    }

    // TODO : 피격대상 이동속도 느려지게함(3초 동안 이속 -40%)

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.layer == 17 || other.gameObject.tag == enemyTag)
            {
                BuffManager.Instance.AddBuff(deBuff);
                SkillDamage(damage, other.gameObject);
                _ability.OnLock(false);
                PhotonNetwork.Destroy(gameObject);
            }

            return;
        }
    }
}
