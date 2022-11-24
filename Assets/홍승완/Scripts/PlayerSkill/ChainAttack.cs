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

    public GameObject[] Effect;

    public PlayerSkillDatas data;

    #region private ��������

    Quaternion quaternion;
    float elapsedTime;
    Vector3 mouseDir;

    private float holdingTime;
    private float damage;
    private float range;
    private float lockTime;

    #endregion


    private void Awake()
    {
        //effect = gameObject.GetComponent<ParticleSystem>();
        for (int i = 0; i < Effect.Length; i++)
        {
            Effect[i].SetActive(false);
        }

    }

    List<int> randomNum = new List<int>();    
    private void OnEnable()
    {
        elapsedTime = 0f;
        damage = SetDamage;
        //Debug.Log($"SetDamage : {SetDamage}");
        holdingTime = SetHodingTime;
        //Debug.Log($"SetHodingTime : {SetHodingTime}");
        range = SetRange;
        //Debug.Log($"SetRange : {SetRange}");
        lockTime = SetLockTime;
        //Debug.Log($"lockTime : {lockTime}");

        int currentNumber = Random.Range(0, 4);

        for (int i = 0; i < 4;)
        {
            if (randomNum.Contains(currentNumber))
            {
                currentNumber = Random.Range(0, 4);
            }
            else
            {
                randomNum.Add(currentNumber);
                i++;
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (_ability == null)
        {
            return;
        }

        //photonView.RPC(nameof(TagProcessing), RpcTarget.All);

        _ability.OnLock(true);
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
            // ��ų���� �÷��̾� ��ġ�� �װ����� ������Ű�� ���ػ��
            quaternion = _ability.transform.localRotation;
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

        // ������ ����Ʈ 4�� ���ִ� �κ�(���� ������â�� �׳�)
        if (testInt <= 3)
        {
            EnableTime += Time.deltaTime;
            if (EnableTime >= 0.1f)
            {
                photonView.RPC(nameof(RPC_RandomVFX), RpcTarget.All, testInt);
                testInt++;
                EnableTime = 0f;
            }
        }

        SkillUpdatePosition();
        SkillHoldingTime(holdingTime);

        dispersionTime += Time.deltaTime;
        
        float tickTime = holdingTime / 10;
        if (dispersionTime >= tickTime)
        {
            dispersionTime = 0f;
            isDamage = true;
        }

    }
    float EnableTime;
    int testInt;

    [PunRPC]
    public void RPC_RandomVFX(int idx)
    {
        Effect[randomNum[idx]].SetActive(true);
    }

    /// <summary>
    /// ��ų�� �÷��̾ �����������
    /// </summary>
    public override void SkillUpdatePosition()
    {
        Debug.Log("��ġ ������Ʈ��");
        transform.position = _ability.transform.position;
        transform.rotation = quaternion;
    }

    public override void SkillHoldingTime(float time)
    {
        elapsedTime += Time.deltaTime;

        // LockŸ�� ���� �ٸ� ��ų�� ������ ���ش�
        if (elapsedTime >= lockTime)
        {
            _ability.OnLock(false);
        }

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
            if (other.GetComponent<Health>() || other.GetComponent<Enemybase>())
            {
                if (isDamage)
                {
                    isDamage = false;
                    float tickDamage = damage / 10;
                    SkillDamage(tickDamage, other.gameObject);
                }
            }
        }
    }
}
