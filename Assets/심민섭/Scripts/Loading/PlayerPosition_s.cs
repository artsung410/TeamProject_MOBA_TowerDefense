/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerPosition_s : MonoBehaviourPun
{
    // 테스트 텍스트
    *//*[SerializeField]
    public Text playerText;*//*
    [SerializeField]
    private GameObject showLoadingCharactor;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("PlayerLoadingImagePositionFromTotal", RpcTarget.Others);
        }
        else
        {
            photonView.RPC("PlayerLoadingImagePositionFromTotal", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void PlayerLoadingImagePositionFromTotal()
    {
        if (photonView.IsMine)
        {
            transform.position = new Vector2(126f, 136f);
        }
        else
        {
            transform.position = new Vector2(1780f, 136f); 
        }
    }



}
*/