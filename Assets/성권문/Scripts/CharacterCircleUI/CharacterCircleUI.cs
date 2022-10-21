using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CharacterCircleUI : MonoBehaviourPun
{
    private void OnEnable()
    {
        transform.Rotate(-90, 0f, 0f);

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.blue;
                Debug.Log("1");
            }

            else
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
                Debug.Log("2");
                transform.parent = GameManager.Instance.CurrentPlayers[1].transform;
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
                Debug.Log("3");
                transform.parent = GameManager.Instance.CurrentPlayers[0].transform;
            }

            else
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.blue;
                Debug.Log("4");
            }
        }
    }
}
