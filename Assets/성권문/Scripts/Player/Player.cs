using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Player : MonoBehaviourPun
{
    public static event Action<Player> PlayerMouseDownEvent = delegate { };
    public Stats playerStats;
    public Health playerHealth;
    public Sprite playerIcon;
    public PlayerBehaviour playerBehaviour;

    private float defaultRecoveryValue = 1.5f;
    private float addRecoveryValue = 0f;
    private float prevMoveSpeed;

    private void Awake()
    {
        BuffManager.playerBuffAdditionEvent += incrementBuffValue;
    }

    private void OnEnable()
    {
        GameManager.Instance.CurrentPlayers.Add(gameObject);
    }
    private void OnMouseDown()
    {
        if (photonView.IsMine)
        {
            return;
        }

        PlayerMouseDownEvent.Invoke(this);

    }

    float elapsedTime;
    private void Update()
    {
        if (photonView.IsMine)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1f)
        {
            playerHealth.Regenation(defaultRecoveryValue + addRecoveryValue);
            elapsedTime = 0f;
        }
    }

    public void incrementBuffValue(int id, float addValue, bool state)
    {
        if (!photonView.IsMine)
        {
            Debug.Log(gameObject.tag + "���� �ƴ�");
            return;
        }

        Debug.Log(gameObject.tag + "���� �ܿ�");

        if (id <= 10000)
        {
            ApplyDebuff(id, addValue, state);
        }
        else
        {
            StartCoroutine(delayApplyBuff(id, addValue, state));
        }
    }

    // �÷��̾� ���� �ʱ�ȭ�� �̷���� �ڿ� ���������
    IEnumerator delayApplyBuff(int id, float addValue, bool state)
    {
        yield return new WaitForSeconds(2f);
        photonView.RPC(nameof(RPC_ApplyBuff), RpcTarget.All, id, addValue, state);
    }

    // ��ų ���� �����
    public void ApplyDebuff(int id, float addValue, bool state)
    {
            photonView.RPC(nameof(RPC_ApplyBuff), RpcTarget.All, id, addValue, state);
    }

// TODO : ���� ��������� �����ϰ� �Ұ�
// TODO : ĳ���� setactive false�ǵ� ��ũ��Ʈ �����ϰ���


[PunRPC]
    public void RPC_ApplyBuff(int id, float addValue, bool state)
    {
        // �÷��̾� ���� ����
        if (id == (int)Buff_Group.Attack_Increase || id == (int)Buff_Group.Attack_Decrese)
        {
            if (state)
            {
                playerStats.attackDmg += addValue;
            }
            else
            {
                playerStats.attackDmg -= addValue;
            }
        }

        else if (id == (int)Buff_Group.HP_Regen_Increase || id == (int)Buff_Group.HP_Regen_Decrease || id == (int)Buff_Group.Burn)
        {
            if (state)
            {
                addRecoveryValue += addValue;
            }
            else
            {
                addRecoveryValue -= addValue;
            }
        }

        else if (id == (int)Buff_Group.Move_Speed_Increase || id == (int)Buff_Group.Move_Speed_Decrese || id == (int)Buff_Group.Freezing)
        {
            if (state)
            {
                playerStats.MoveSpeed *= (1 + addValue); // ��������
            }
            else
            {
                playerStats.MoveSpeed /= (1 + addValue);  // ���󺹱�
            }
        }

        else if (id == (int)Buff_Group.Attack_Speed_Increase || id == (int)Buff_Group.Attack_Speed_Decrese)
        {
            if (state)
            {
                playerStats.attackSpeed *= addValue;
            }
            else
            {
                playerStats.attackSpeed /= addValue;
            }
        }

        else if (id == (int)Buff_Group.Stun)
        {
            if (state)
            {
                playerBehaviour.OnStun(state, 10f);
            }
            else
            {
                playerBehaviour.OnStun(state, 0f);
            }
        }

    }
}