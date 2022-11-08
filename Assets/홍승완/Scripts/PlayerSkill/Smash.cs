using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Smash : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    #region private ��������

    Quaternion quaternion;
    float elapsedTime;
    string enemyTag;
    Vector3 mouseDir;

    private float HoldingTime;
    private float Damage;
    private float Range;
    float angle;
    #endregion

    private void OnEnable()
    {
        elapsedTime = 0f;
        Damage = SetDamage;
        HoldingTime = SetHodingTime;
        Range = SetRange;

        angle = 120f;
    }

    void Start()
    {
        if (_ability == null)
        {
            return;
        }

        LookMouseCursor();
        TagProcessing(_ability);
    }


    public void LookMouseCursor()
    {
        // ���콺 ���⿡�� ���
        RaycastHit hit;
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z) - _ability.transform.position;

            _ability.transform.forward = mouseDir;
            // ��ų���� �÷��̾� ��ġ�� �װ����� ������Ű�� ���ػ��
            quaternion = _ability.transform.localRotation;
        }

    }

    private void TagProcessing(HeroAbility ability)
    {

        if (ability.CompareTag("Blue"))
        {
            enemyTag = "Red";
            //Debug.Log(enemyTag);
        }
        else if (ability.CompareTag("Red"))
        {
            enemyTag = "Blue";
            //Debug.Log(enemyTag);

        }
    }

    private void Update()
    {
        if (_ability == null)
        {
            return;
        }

        if (photonView.IsMine)
        {
            SkillUpdatePosition();
            SkillHoldingTime(HoldingTime);
        }
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // ���ӽð����� �÷��̾ �������� ���Ѵ�
        _behaviour.targetedEnemy = null;
        _behaviour.perfomMeleeAttack = false;

        _ability.transform.rotation = quaternion;

        // ��ų�� ���� �÷��̾�� ���ڸ����� �����
        _behaviour.ForSkillAgent(_behaviour.transform.position);

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
        transform.position = _behaviour.transform.position;
        transform.rotation = quaternion;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.CompareTag(enemyTag))
            {
                SectorDamage(other);
            }
        }
    }

    private void SectorDamage(Collider other)
    {
        Vector3 interV = other.gameObject.transform.position - _behaviour.gameObject.transform.position;

        if (interV.magnitude <= Range)
        {
            float dot = Vector3.Dot(interV.normalized, _behaviour.gameObject.transform.forward);
            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            if (degree <= angle / 2f)
            {
                SkillDamage(Damage, other.gameObject);
            }
        }
    }

}
