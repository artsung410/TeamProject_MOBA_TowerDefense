using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerPosition : MonoBehaviourPun
{
    // 테스트 텍스트
    [SerializeField]
    public Text playerText;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            /*transform.position = new Vector2(0f, -2.4f);
            playerText.text = 11.ToString();*/
            photonView.RPC("PlayerLoadingImagePositionFromTotal", RpcTarget.Others);
        }
        else
        {
            /*transform.position = new Vector2(0f, 4f);
            playerText.text = 22.ToString();*/
            photonView.RPC("PlayerLoadingImagePositionFromTotal", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void PlayerLoadingImagePositionFromTotal()
    {
        if (photonView.IsMine)
        {
            transform.position = new Vector2(0f, -2.4f);
            playerText.text = 11.ToString();
        }
        else
        {
            transform.position = new Vector2(0f, 4f);
            playerText.text = 22.ToString();
        }
    }

    /*[PunRPC]
    private void PlayerLoadingImagePositionFromMaster()
    {
        transform.position = new Vector2(0f, 4f);
        playerText.text = 22.ToString();
    }

    [PunRPC]
    private void PlayerLoadingImagePositionFromClient()
    {
        transform.position = new Vector2(0f, -2.4f);
        playerText.text = 11.ToString();
    }*/

}
