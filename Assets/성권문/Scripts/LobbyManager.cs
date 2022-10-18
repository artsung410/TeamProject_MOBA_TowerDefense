using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";

    public TextMeshProUGUI connectionInfoText; // ��Ʈ��ũ ���� �ؽ�Ʈ
    public Button joinButton;
    public GameObject playerStoragePre;

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
        Instantiate(playerStoragePre, Vector3.zero, Quaternion.identity);
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
        TrojanHorse tro = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();
        tro.PlayerTrojanInfo();

        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "Connecting to Random Room...";
            // ������ �������� GetCaller�� �ִ� �۾��� ���⼭ �Ѵ�.

            // -----------------------------------------------------
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

        // �׷��� LoadLevel�� �̿��ؼ� ���� ����ȭ�� �ʿ䰡 ����
        PhotonNetwork.LoadLevel("Prototype_1");
    }
}
