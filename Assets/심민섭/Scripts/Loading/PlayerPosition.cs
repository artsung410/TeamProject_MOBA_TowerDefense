/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerPosition : MonoBehaviourPun
{
    // �׽�Ʈ �ؽ�Ʈ
    *//*[SerializeField]
    public Text playerText;*//*

    void Start()
    {

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1) // ������
        {
            photonView.RPC("PlayerLoadingImagePositionFromTotal", RpcTarget.All);
        }
        else // 2 �϶� Ŭ��
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
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1) // ������
        {
            transform.position = new Vector2(725f, 570f);
            //playerText.text = 11.ToString();
        }
        else // 2 �϶� Ŭ��
        {
            transform.position = new Vector2(1200f, 570f);
            //playerText.text = 22.ToString();
        }
    }
}
*/