using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Whirlwind : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public GameObject damageZone;

    #region Private ������

    Quaternion quaternion;
    float elapsedTime;
    //string enemyTag;
    Vector3 mouseDir;

    private float damage;
    private float speed;
    private float vertical;
    #endregion

    // TODO : ������ ����Ʈ�κ� �����ʿ�

    private void Awake()
    {
        vertical = damageZone.GetComponent<BoxCollider>().size.z;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        damage = Data.Value_1;
        vertical = Data.Range;
        speed = 500f;
    }

    private void Start()
    {
        if (_ability == null)
        {
            return;
        }

        _ability.OnLock(true);

    }

    private void Update()
    {
        if (_ability == null)
        {
            return;
        }

        if (_ability.gameObject.GetComponent<Health>().isDeath == true)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (photonView.IsMine)
        {

            SkillUpdatePosition();
            SkillHoldingTime(Data.HoldingTime);
        }
    }
    public override void SkillUpdatePosition()
    {
        // ��ų�� �÷��̾� �ֺ��� �ִ�
        transform.position = _ability.gameObject.transform.position;
        // ȸ���� �Ѵ�
        damageZone.transform.Rotate(speed * Time.deltaTime * Vector3.up);

        
        
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // TODO : ���� ����� �ִϸ��̼����� ó���Ұ�

        // ���ӽð����� �÷��̾ �������� ���Ѵ�
        _behaviour.targetedEnemy = null;
        _behaviour.perfomMeleeAttack = false;

        if (elapsedTime >= Data.LockTime)
        {
            _ability.OnLock(false);
        }

        // �˱� ���ӽð��� ������ ��������Ѵ�
        if (elapsedTime >= time)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                BuffBlueprint buff = CSVtest.Instance.BuffDic[Data.ID + 50];
                if (other.gameObject.layer == 7 && other.CompareTag(enemyTag))
                {
                    BuffManager.Instance.AddBuff(buff);
                }

                SkillDamage(damage, other.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
