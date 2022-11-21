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
    string enemyTag;
    Vector3 mouseDir;

    private float holdingTime;
    private float damage;
    private float range;
    private float speed;
    private float lockTime;
    #endregion

    // TODO : ������ ����Ʈ�κ� �����ʿ�

    private void OnEnable()
    {
        elapsedTime = 0f;
        damage = SetDamage;
        holdingTime = SetHodingTime;
        range = SetRange;
        speed = 1000f;
        lockTime = SetLockTime;
    }

    private void Start()
    {
        if (_ability == null)
        {
            return;
        }

        _ability.OnLock(true);
        TagProcessing(_ability);
        //LookMouseCursor();
    }

    public void LookMouseCursor()
    {
        // ���콺 ���⿡�� ���
        RaycastHit hit;
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z) - _ability.transform.position;

            _ability.transform.forward = mouseDir;
            quaternion = _ability.transform.localRotation;
        }
    }
    private void TagProcessing(HeroAbility ability)
    {

        if (ability.CompareTag("Blue"))
        {
            enemyTag = "Red";
        }
        else if (ability.CompareTag("Red"))
        {
            enemyTag = "Blue";
        }
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
            SkillHoldingTime(holdingTime);
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

        // ���ӽð����� �÷��̾ ��������
        _stat.MoveSpeed = 8f;

        // TODO : ���� ����� �ִϸ��̼����� ó���Ұ�
        // �÷��̾ �ѹ� ��������
        //_behaviour.gameObject.transform.Rotate(Vector3.up * 3000 * Time.deltaTime);

        // ���ӽð����� �÷��̾ �������� ���Ѵ�
        _behaviour.targetedEnemy = null;
        _behaviour.perfomMeleeAttack = false;

        if (elapsedTime >= lockTime)
        {
            _ability.OnLock(false);
        }

        // �˱� ���ӽð��� ������ ��������Ѵ�
        if (elapsedTime >= time)
        {
            _stat.MoveSpeed = 15f;

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
            if (other.CompareTag(enemyTag))
            {
                SkillDamage(damage, other.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
