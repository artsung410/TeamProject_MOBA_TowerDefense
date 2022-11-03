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
        photonView.RPC("RPC_ApplyPlayerBuff", RpcTarget.All, id, addValue, state);
    }

    [PunRPC]
    public void RPC_ApplyPlayerBuff(int id, float value, bool st)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (id == (int)Buff_Effect.AtkUP)
        {
        }

        else if (id == (int)Buff_Effect.AtkSpeedUp)
        {
        }

        else if (id == (int)Buff_Effect.HpUp)
        {
            if (st)
            {
                Debug.Log("플레이어 최대 체력 증가!");
                playerHealth.hpSlider3D.maxValue += value;
                playerHealth.hpSlider3D.value += value;
            }
            else
            {
                Debug.Log("플레이어 최대 체력 증가 종료!");
                playerHealth.hpSlider3D.maxValue -= value;
                playerHealth.hpSlider3D.value += value;
            }
        }
    }
}