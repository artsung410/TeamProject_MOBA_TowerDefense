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
    public static GameExitButton Instance;

    [Header("GameESCUI")]
    public GameObject ESCButton;
    private bool openESCWindow = false;

    private void Awake()
    {
        Instance = this;
    }

    public void LeaveRoom()
    {
        if (!GameManager.Instance.isGameEnd)
        {
            Destroy(GameObject.FindGameObjectWithTag("APIStorage").gameObject);
            Destroy(GameObject.FindGameObjectWithTag("GetCaller").gameObject);
        }

        photonView.RPC("PlayerLeaveGameRoom", RpcTarget.All);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            openESCWindow = !openESCWindow;
            ESCButton.SetActive(openESCWindow);
        }
    }

    private void ESCButton_S()
    {
        ESCButton.SetActive(true);
    }


    public override void OnLeftRoom()
    {
        if (SceneManager.GetActiveScene().name == "demoSmaller")
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
