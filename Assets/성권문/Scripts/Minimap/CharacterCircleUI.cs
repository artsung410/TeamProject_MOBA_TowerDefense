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

    private void OnEnable()
    {
        transform.Rotate(-90, -135f, 0f);
        PrevRotation = transform.rotation;

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.blue;
                transform.parent = GameManager.Instance.CurrentPlayers[0].transform;
            }

            else
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
                transform.parent = GameManager.Instance.CurrentPlayers[1].transform;
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
                transform.parent = GameManager.Instance.CurrentPlayers[0].transform;
            }

            else
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.blue;
                transform.parent = GameManager.Instance.CurrentPlayers[1].transform;
            }
        }

    }
    private void Update()
    {
        transform.rotation = PrevRotation;
    }
}
