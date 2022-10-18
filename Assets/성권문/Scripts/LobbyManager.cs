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

    public TextMeshProUGUI connectionInfoText; // 네트워크 상태 텍스트
    public Button joinButton;
    public Button EnterButton;
    private void Start()
    {
        
        // 포톤 네트워크에 게임 버전을 명시해주어야함
        PhotonNetwork.GameVersion = gameVersion;

        // 마스터 서버에 접속을 시도함.
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text = "Connecting To Master Server...";
    }

    // 마스터 서버에 접속 시도 / 자동실행
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected to Master Server";
    }

    // 연결이 끊켰을경우 / 자동실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline : Connection Disabled {cause.ToString()}";

        // 재접속 시도
        PhotonNetwork.ConnectUsingSettings();

    }


    // join button을 눌렀을때 메소드
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

            // 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "There is no empty room, Creating new Room.";

        // 네트워크 상에서 인원2인 방을 자동으로 생성함.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with Room.";

        // 로드씬으로 진행하면 나만 넘어가고, 방에있는사람은 안넘어감..

        
        PhotonNetwork.LoadLevel("Prototype_1");
       
        
        // 그래서 LoadLevel을 이용해서 씬을 동기화할 필요가 있음
    }
}
