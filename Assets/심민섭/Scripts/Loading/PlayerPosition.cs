/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerPosition : MonoBehaviourPun
{
    // 테스트 텍스트
    *//*[SerializeField]
    public Text playerText;*//*

    void Start()
    {

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1) // 마스터
        {
            photonView.RPC("PlayerLoadingImagePositionFromTotal", RpcTarget.All);
        }
        else // 2 일때 클라
        {
            photonView.RPC("PlayerLoadingImagePositionFromTotal", RpcTarget.All);
        }

        *//*if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("PlayerLoadingImagePositionFromTotal", RpcTarget.Others);
        }
        else
        {
            photonView.RPC("PlayerLoadingImagePositionFromTotal", RpcTarget.MasterClient);
        }*//*
    }

    [PunRPC]
    private void PlayerLoadingImagePositionFromTotal()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1) // 마스터
        {
            transform.position = new Vector2(725f, 570f);
            //playerText.text = 11.ToString();
        }
        else // 2 일때 클라
        {
            transform.position = new Vector2(1200f, 570f);
            //playerText.text = 22.ToString();
        }
    }
}
*/