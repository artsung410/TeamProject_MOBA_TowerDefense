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

    #endregion

    public float HoldingTime;
    public float Damage;
    public float Range;

    private void Awake()
    {
        effect = gameObject.GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_ability == null)
        {
            return;
        }

        

        // ���콺 ���⿡�� ���
        RaycastHit hit;
        if (Physics.Raycast(_behaviour.ray, out hit))
        {
            mouseDir = new Vector3(hit.point.x, _ability.transform.position.y, hit.point.z) - _ability.transform.position;

            _ability.transform.forward = mouseDir;
            // ��ų���� �÷��̾� ��ġ�� �װ����� ������Ű�� ���ػ��
            quaternion = _ability.transform.localRotation;
        }

        //photonView.RPC(nameof(TagProcessing), RpcTarget.All);
        TagProcessing(_ability);

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
    // Update is called once per frame
    void Update()
    {
        if (_ability == null)
        {
            return;
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
    private void SkillUpdatePosition()
    {
        transform.position = _ability.transform.position;
        transform.rotation = quaternion;
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
                    //cnt++;
                    //Debug.Log(cnt);
                }
            }
        }
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // ���ӽð����� �÷��̾ ��������
        _stat.MoveSpeed = 3f;

        // ���ӽð����� �÷��̾�� ��ų���⸸ �ٶ󺻴�
        _ability.transform.rotation = quaternion;

        // ���ӽð��� ������ �������
        if (elapsedTime >= time)
        {
            _stat.MoveSpeed = 10f;
            // Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left �����߻�
            // �ذ�� : photonView.IsMine ���� �߰�
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
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
        else if (target.gameObject.layer == 8)
        {
            Enemybase minion = target.GetComponent<Enemybase>();


            if (minion != null)
            {
                minion.TakeDamage(damage);
            }
        }
    }

}
