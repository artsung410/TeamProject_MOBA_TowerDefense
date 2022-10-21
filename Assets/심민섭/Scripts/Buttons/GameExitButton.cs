using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameExitButton : MonoBehaviourPunCallbacks
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // SMS start -------------------------------------------------------
    // 게임에서 나가기 설정 (한명이 탈주하면 같은 방 사람들 모두 나가짐)
    public void LeaveRoom()
    {
        photonView.RPC("PlayerLeaveGameRoom", RpcTarget.All);
    }

    public override void OnLeftRoom()
    {
        if (SceneManager.GetActiveScene().name == "Prototype_1")
        {
            SceneManager.LoadScene("Lobby");
            return;
        }
    }

    public void TargetLeaveRoom()
    {
        photonView.RPC("PlayerLeaveGameRoom", RpcTarget.Others);
    }

    [PunRPC]
    public void PlayerLeaveGameRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    // SMS end -------------------------------------------------------
}
