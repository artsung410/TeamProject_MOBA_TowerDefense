using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpritSword : SkillHandler
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    #region Private ������
    
    Quaternion quaternion;
    float elapsedTime;
    string enemyTag;
    Vector3 mouseDir;

    #endregion

    private float HoldingTime;
    private float Damage;
    private float Range;
    public float Speed;

    private void OnEnable()
    {
        elapsedTime = 0f;
        Damage = SetDamage;
        HoldingTime = SetHodingTime;
        Range = SetRange;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_ability == null)
        {
            return;
        }

        TagProcessing(_ability);
        LookMouseCursor();
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
            //Debug.Log(enemyTag);
        }
        else if (ability.CompareTag("Red"))
        {
            enemyTag = "Blue";
            //Debug.Log(enemyTag);

        }
    }

    // Update is called once per frame
    void Update()
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

    public void SkillUpdatePosition()
    {
        // �߻�ü�� ������ ���ư��Բ� �Ѵ�
        transform.Translate(Time.deltaTime * Speed * Vector3.forward);

        // ȸ���κ��� ó��ȸ����ġ���� ���ư���
        transform.rotation = quaternion;
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
            Debug.Log("1. ����");
            Enemybase minion = target.GetComponent<Enemybase>();

            if (minion != null)
            {
                Debug.Log("2. ������@");

                minion.TakeDamage(damage);
            }
        }
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

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
            if (other.CompareTag(enemyTag))
            {
                SkillDamage(Damage, other.gameObject);
            }
        }
    }
}
