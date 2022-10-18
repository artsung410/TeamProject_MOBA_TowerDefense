using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";

    public TextMeshProUGUI connectionInfoText; // ��Ʈ��ũ ���� �ؽ�Ʈ
    public Button joinButton;
    public Button EnterButton;
    private void Start()
    {
        
        // ���� ��Ʈ��ũ�� ���� ������ ������־����
        PhotonNetwork.GameVersion = gameVersion;

        // ������ ������ ������ �õ���.
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text = "Connecting To Master Server...";
    }

    // ������ ������ ���� �õ� / �ڵ�����
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected to Master Server";
    }

    // ������ ��������� / �ڵ�����
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline : Connection Disabled {cause.ToString()}";

        // ������ �õ�
        PhotonNetwork.ConnectUsingSettings();

    }


    // join button�� �������� �޼ҵ�
    public void Connect()
    {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "Connecting to Random Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = $"Offline : Connection Disabled - Try reconnecting..";

            // ������ �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "There is no empty room, Creating new Room.";

        // ��Ʈ��ũ �󿡼� �ο�2�� ���� �ڵ����� ������.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with Room.";

        // �ε������ �����ϸ� ���� �Ѿ��, �濡�ִ»���� �ȳѾ..

        
        PhotonNetwork.LoadLevel("Prototype_1");
       
        
        // �׷��� LoadLevel�� �̿��ؼ� ���� ����ȭ�� �ʿ䰡 ����
    }
}
