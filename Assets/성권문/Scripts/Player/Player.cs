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
        if(photonView.IsMine)
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
        StartCoroutine(delayApplyBuff(id, addValue, state));
    }

    // �÷��̾� ���� �ʱ�ȭ�� �̷���� �ڿ� ���������
    IEnumerator delayApplyBuff(int id, float addValue, bool state)
    {
        yield return new WaitForSeconds(2f);
        photonView.RPC(nameof(RPC_ApplyBuff), RpcTarget.All, id, addValue, state);
    }

    // TODO : ���� ��������� �����ϰ� �Ұ�
    // TODO : ĳ���� setactive false�ǵ� ��ũ��Ʈ �����ϰ���
    [PunRPC]
    public void RPC_ApplyBuff(int id, float addValue, bool state)
    {
        // �÷��̾� ���� ����
        if (id == (int)Buff_Effect_byTower.AtkUP || id == (int)Buff_Effect_byTower.AtkDown)
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

        else if (id == (int)Buff_Effect_byTower.HpRegenUp || id == (int)Buff_Effect_byTower.HpRegenDown)
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

        else if (id == (int)Buff_Effect_byTower.MoveSpeedUp || id == (int)Buff_Effect_byTower.MoveSpeedDown)
        {
            if (state)
            {
                playerStats.MoveSpeed += addValue;
            }
            else
            {
                playerStats.MoveSpeed -= addValue;
            }
        }

        else if (id == (int)Buff_Effect_byTower.AtkSpeedUp || id == (int)Buff_Effect_byTower.AtkSpeedDown)
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