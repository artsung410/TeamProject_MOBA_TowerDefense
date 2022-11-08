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

    // 플레이어 버프효과 발동
    public void incrementBuffValue(int id, float addValue, bool state)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("RPC_ApplyBuff", RpcTarget.All, id, addValue, state);
    }

    [PunRPC]
    public void RPC_ApplyBuff(int id, float addValue, bool state)
    {
        if (id == (int)Buff_Effect.AtkUP)
        {
            if (state)
            {
                Debug.Log("플레이어 공격력 증가!");
                playerStats.attackDmg += addValue;
            }
            else
            {
                Debug.Log("플레이어 공격력 증가 종료!");
                playerStats.attackDmg -= addValue;
            }
        }

        else if (id == (int)Buff_Effect.AtkSpeedUp)
        {
        }

        else if (id == (int)Buff_Effect.HpUp)
        {
            if (state)
            {
                Debug.Log("플레이어 최대 체력 증가!");
                playerHealth.hpSlider3D.maxValue += addValue;
                playerHealth.hpSlider3D.value += addValue;
            }
            else
            {
                Debug.Log("플레이어 최대 체력 증가 종료!");
                playerHealth.hpSlider3D.maxValue -= addValue;
                playerHealth.hpSlider3D.value += addValue;
            }
        }
    }
}