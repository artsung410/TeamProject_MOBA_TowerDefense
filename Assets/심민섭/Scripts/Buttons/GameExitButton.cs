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

    // �������� ���ӿ��� ������ ���� (�Ѹ��� Ż���ϸ� ���� �� ����� ��� ������)
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

    // �ʿ� ����
    /*public void TargetLeaveRoom()
    {
        photonView.RPC("PlayerLeaveGameRoom", RpcTarget.Others);
    }*/

    [PunRPC]
    public void PlayerLeaveGameRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    // ������������ ���ӿ��� ������ ��쿡�� �״�� ������ �����ϰ� �ؼ����� �ı��� ���� �̱�� ������ �ϸ�Ǵϱ� ���� ����



    // �ӽ� �ڵ�
    //OnPhotonPlayerDisconnected �Լ��� ���� ���� ��� �÷��̾ ������ �������� �� ȣ�� �˴ϴ�.
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
