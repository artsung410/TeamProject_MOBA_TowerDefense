using Photon.Pun;
using UnityEngine;
using System;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Player : MonoBehaviourPun
{
    public static event Action<Stats> PlayerMouseDownEvent = delegate { };
    public Stats playerStats;
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

        PlayerMouseDownEvent.Invoke(playerStats);
        //PlayerHUD.Instance.ActivationEnemyInfoUI();
    }
}