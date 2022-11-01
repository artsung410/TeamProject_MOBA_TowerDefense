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

    private float HoldingTime;
    private float Damage;
    private float Range;
    private float Speed;
    #endregion

    // TODO : ������ ����Ʈ�κ� �����ʿ�

    private void OnEnable()
    {
        elapsedTime = 0f;
        Damage = SetDamage;
        HoldingTime = SetHodingTime;
        Range = SetRange;
        Speed = 1000f;
    }

    private void Start()
    {
        if (_ability == null)
        {
            return;
        }

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
            SkillHoldingTime(HoldingTime);
        }
    }
    public void SkillUpdatePosition()
    {
        // ��ų�� �÷��̾� �ֺ��� �ִ�
        transform.position = _ability.gameObject.transform.position;
        // ȸ���� �Ѵ�
        damageZone.transform.Rotate(Speed * Time.deltaTime * Vector3.up);

        
        
    }

    public override void SkillDamage(float damage, GameObject target)
    {
        if (target.gameObject.layer == 7)
        {
            Health player = target.GetComponent<Health>();

            if (player != null)
            {
                player.OnDamage(damage);
            }
        }
        else if (target.gameObject.layer == 8 || target.gameObject.layer == 13)
        {
            Enemybase minion = target.GetComponent<Enemybase>();

            if (minion != null)
            {
                minion.TakeDamage(damage);
            }
        }
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // ���ӽð����� �÷��̾ ��������
        _stat.MoveSpeed = 8f;

        // �÷��̾ �ѹ� ��������
        _behaviour.gameObject.transform.Rotate(Vector3.up * 3000 * Time.deltaTime);

        // ���ӽð����� �÷��̾ �������� ���Ѵ�
        _behaviour.targetedEnemy = null;
        _behaviour.perfomMeleeAttack = false;

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
                SkillDamage(Damage, other.gameObject);
            }
        }
    }
}
