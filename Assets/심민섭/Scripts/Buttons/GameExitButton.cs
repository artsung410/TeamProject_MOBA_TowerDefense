using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameExitButton : MonoBehaviourPunCallbacks
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 정상적인 게임에서 나가기 설정 (한명이 탈주하면 같은 방 사람들 모두 나가짐)
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

    // 필요 없음
    /*public void TargetLeaveRoom()
    {
        photonView.RPC("PlayerLeaveGameRoom", RpcTarget.Others);
    }*/

    [PunRPC]
    public void PlayerLeaveGameRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    // 비정상적으로 게임에서 나가는 경우에는 그대로 게임을 진행하고 넥서스를 파괴한 쪽이 이기는 것으로 하면되니까 문제 없음



    // 임시 코드
    //OnPhotonPlayerDisconnected 함수는 게임 도중 상대 플레이어가 접속이 끊어졌을 때 호출 됩니다.
    /*public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("other player disconnected");
        }
        else
        {
            Debug.Log("master disconnected");
        }
    }*/



}
