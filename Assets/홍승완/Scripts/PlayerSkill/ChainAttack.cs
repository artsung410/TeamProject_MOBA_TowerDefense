using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChainAttack : SkillHandler
{

    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public ParticleSystem effect;

    #region private ��������

    Quaternion quaternion;
    float elapsedTime;
    string enemyTag;
    Vector3 mouseDir;

    private float HoldingTime;
    private float Damage;
    private float Range;

    #endregion


    private void Awake()
    {
        //effect = gameObject.GetComponent<ParticleSystem>();
    }

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

        //photonView.RPC(nameof(TagProcessing), RpcTarget.All);
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

    float dispersionTime = 0f;
    bool isDamage = true;
    void Update()
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

        SkillUpdatePosition();
        SkillHoldingTime(HoldingTime);

        dispersionTime += Time.deltaTime;
        float tickTime = HoldingTime / 10;
        if (dispersionTime >= tickTime)
        {
            dispersionTime = 0f;
            isDamage = true;
        }
    }

    /// <summary>
    /// ��ų�� �÷��̾ �����������
    /// </summary>
    public override void SkillUpdatePosition()
    {
        transform.position = _ability.transform.position;
        transform.rotation = quaternion;
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // ���ӽð����� �÷��̾ ��������
        _stat.MoveSpeed = 3f;

        // ���ӽð����� �÷��̾ �������� ���Ѵ�
        _behaviour.targetedEnemy = null;
        _behaviour.perfomMeleeAttack = false;

        // ���ӽð����� �÷��̾�� ��ų���⸸ �ٶ󺻴�
        _ability.transform.rotation = quaternion;

        // ���ӽð��� ������ �������
        if (elapsedTime >= time)
        {
            _stat.MoveSpeed = 15f;
            // Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left �����߻�
            // �ذ�� : photonView.IsMine ���� �߰�
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // ������ �ι������κ� IsMine���� ó��
        if (photonView.IsMine)
        {
            if (other.CompareTag(enemyTag))
            {
                if (isDamage)
                {
                    isDamage = false;
                    float tickDamage = Damage / 10;
                    SkillDamage(tickDamage, other.gameObject);
                }
            }
        }
    }
}
