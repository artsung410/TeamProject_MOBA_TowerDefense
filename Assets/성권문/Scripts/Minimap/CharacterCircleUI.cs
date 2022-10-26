using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CharacterCircleUI : MonoBehaviourPun
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    Quaternion PrevRotation;
    GameObject player;
    private void OnEnable()
    {
        transform.Rotate(-90, -135f, 0f);
        PrevRotation = transform.rotation;

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.blue;
                player = GameManager.Instance.CurrentPlayers[0];
            }

            else
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
                player = GameManager.Instance.CurrentPlayers[1];
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
                player = GameManager.Instance.CurrentPlayers[0];
            }

            else
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.blue;
                player = GameManager.Instance.CurrentPlayers[1];
            }
        }
    }

    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 100, player.transform.position.z);
    }
}
