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
    public static event Action<Stats, Sprite> PlayerMouseDownEvent = delegate { };
    public Stats playerStats;
    public Sprite playerIcon;

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

        PlayerMouseDownEvent.Invoke(playerStats, playerIcon);
        //PlayerHUD.Instance.ActivationEnemyInfoUI();
    }
}