using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MinionCircleUI : MonoBehaviourPun
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################
    [SerializeField]
    Transform minionPosUi;
    private void OnEnable()
    {
        transform.Rotate(0f, 0f, 0f);

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.blue;
            }

            else
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.red;
            }

            else
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.blue;
            }
        }
    }
    private void Update()
    {
         if (transform == null || transform.gameObject == null || this == null)
        {
            return;
        }
        if (transform.parent == null)
        {
            Destroy(gameObject.GetComponent<RectTransform>());
        }
        gameObject.transform.position = new Vector3(minionPosUi.position.x,50, minionPosUi.position.z);
    }
}