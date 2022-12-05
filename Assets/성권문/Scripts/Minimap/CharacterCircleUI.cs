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
    public Image heroImg;

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(new Vector3(-90f, -45f, -135f));
        PrevRotation = transform.rotation;

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                transform.GetChild(1).GetComponent<Image>().color = Color.blue;
                player = GameManager.Instance.CurrentPlayers[0];
                heroImg.sprite = player.GetComponent<Player>().playerIcon;
            }

            else
            {
                transform.GetChild(1).GetComponent<Image>().color = Color.red;
                player = GameManager.Instance.CurrentPlayers[1];
                heroImg.sprite = player.GetComponent<Player>().playerIcon;

            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                transform.GetChild(1).GetComponent<Image>().color = Color.red;
                player = GameManager.Instance.CurrentPlayers[0];
                heroImg.sprite = player.GetComponent<Player>().playerIcon;

            }

            else
            {
                transform.GetChild(1).GetComponent<Image>().color = Color.blue;
                player = GameManager.Instance.CurrentPlayers[1];
                heroImg.sprite = player.GetComponent<Player>().playerIcon;

            }
        }
    }

    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 50f, player.transform.position.z);
    }
}

