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
    public float xSize; // ����
    public float zSize; // ����?

    Quaternion quaternion;
    float elasedTiem;
    string enemyTag;
    Vector3 mouseDir;
    Vector3 currentPos; // ��ų ��� ��ġ

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

    // 10�� ���ư��� ����ü�� HoldingTime�� �ǹ̰� �ִ°ǰ�?
    public override void SkillHoldingTime(float time)
    {
        throw new System.NotImplementedException();
    }

    public override void SkillUpdatePosition()
    {
        transform.Translate(Time.deltaTime * 10f * Vector3.forward);
        transform.rotation = quaternion;
    }

    // TODO : �ǰݴ�� �̵��ӵ� ����������(3�� ���� �̼� -40%)

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
