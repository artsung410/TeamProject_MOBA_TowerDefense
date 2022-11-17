//#define VER_Y_0
#define VER_Y_PLAYER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Blaze : SkillHandler
{


    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject DamageZone;

    Quaternion quaternion;
    float elapsedTime;
    string enemyTag;
    Vector3 mouseDir;

    private float HoldingTime;
    private float Damage;
    private float Range;
    private float Speed;

    float width;
    float length;
    float zCenter;

    private void Awake()
    {
        width = DamageZone.GetComponent<BoxCollider>().size.x;
        length = DamageZone.GetComponent<BoxCollider>().size.z;
        zCenter = DamageZone.GetComponent<BoxCollider>().center.z;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        Damage = SetDamage;
        HoldingTime = SetHodingTime;
        Range = SetRange;

        width = Range;
        length = 3;

        zCenter = width / 2f;
    }

    void Start()
    {
        try
        {
            TagAssignment();
            LookMouseDir();
        }
        catch (System.Exception)
        {
            print("����Ʈ ��ų null������");
        }
    }

    private void TagAssignment()
    {
        if (_ability.CompareTag("Blue"))
        {
            enemyTag = "Red";
        }
        else
        {
            enemyTag = "Blue";
        }
    }

    private void LookMouseDir()
    {
        RaycastHit hit;
#if VER_Y_PLAYER
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _behaviour.transform.position.y, hit.point.z) - _behaviour.transform.position;

            _behaviour.transform.forward = mouseDir;
            quaternion = _behaviour.transform.localRotation;
        }
#endif

#if VER_Y_0
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, 0, hit.point.z) - _behaviour.transform.position;

            _behaviour.transform.forward = mouseDir;
            quaternion = _behaviour.transform.localRotation;
        }
#endif
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            SkillUpdatePosition();
            SkillHoldingTime(HoldingTime);
        }
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= time)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public override void SkillUpdatePosition()
    {
        transform.rotation = quaternion;
    }

    // TODO : ���� ������ �߰��Ұ�
    /*
    ������ ��ų ����
    �÷��̾� ������ �����ȴ�
    ������� 12 * 3 �簢���̴�
    ���ӽð�(2��)���� �ٴڿ� ����ִ�
    �������ذ� �����Ѵ�
        5�ʵ��� 5ƽ -> 1�ʿ� 1ƽ 30������
        �������ش� �ѹ��� ����
     */

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.CompareTag(enemyTag) || other.gameObject.layer == 17)
            {
                SkillDamage(Damage, other.gameObject);
            }
        }
    }
}
