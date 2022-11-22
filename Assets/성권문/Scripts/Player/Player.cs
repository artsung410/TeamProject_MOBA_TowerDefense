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

    private float defaultRecoveryValue = 1.5f;
    private float addRecoveryValue = 0f;
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

    // =========================== Ÿ�� ���� ���� ó�� ===========================
    public void incrementBuffValue(int id, float addValue, bool state)
    {
        if (!photonView.IsMine)
        {
            Debug.Log(gameObject.tag + "���� �ƴ�");
            return;
        }

        Debug.Log(gameObject.tag + "���� �ܿ�");
        
        // ��ų�� id : 9 ~
        if (id >= 9)
        {
            ApplyDebuff(id, addValue, state);
        }
        // Ÿ���� id : 1 ~ 8
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
        Debug.Log($"���� ����\n" +
            $" id : {id}\n" +
            $"addValue : {addValue}\n" +
            $"state : {state}");

        // �÷��̾� ���� ����
        if (id == (int)Buff_Effect.AtkUP || id == (int)Buff_Effect.AtkDown)
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

        else if (id == (int)Buff_Effect.HpRegenUp || id == (int)Buff_Effect.HpRegenDown)
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

        else if (id == (int)Buff_Effect.MoveSpeedUp || id == (int)Buff_Effect.MoveSpeedDown || id == (int)Buff_Effect.SlowOfSkill)
        {
            if (state)
            {
                playerStats.MoveSpeed *= (1 + addValue); // ��������
                Debug.Log($"���� �̵��ӵ� : {playerStats.MoveSpeed}");
            }
            else
            {
                playerStats.MoveSpeed /= (1 + addValue);  // ���󺹱�
                Debug.Log($"���� �̵��ӵ� : {playerStats.MoveSpeed}");
            }
        }

        else if (id == (int)Buff_Effect.AtkSpeedUp || id == (int)Buff_Effect.AtkSpeedDown)
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

    }
}