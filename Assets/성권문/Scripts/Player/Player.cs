using Photon.Pun;
using UnityEngine;
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

    // =========================== Ÿ�� ���� ���� ó�� ===========================
    public void incrementBuffValue(int id, float addValue, bool state)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
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
                playerStats.attackSpeed += addValue;
            }
            else
            {
                playerStats.attackSpeed -= addValue;
            }
        }
    }
}