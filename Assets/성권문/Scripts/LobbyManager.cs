using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
//             MAIL : minsub4400@gmail.com         
// ###############################################

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";

    // SMS start -----------------------------------------------------
    // 최대 인원 수
    private int roomMaxPlayers = 2;
    // 게임 진행 시간
    private int maxTime = 600; // 10분
    // 매칭 중.. 인원수 나타낼 텍스트
    [SerializeField]
    private Text currentPlayerCount;
    // 마스터 서버에 연결되어있는 확인할 변수
    private bool isMasterServerConnect = false;
    // 매칭 시 보여줄 텍스트
    public GameObject matChingObj;
    // SMS end -------------------------------------------------------
    
    public string apiKey = "5hO4J33kQPhtHhq4e0F76V";

    public TextMeshProUGUI connectionInfoText; // 네트워크 상태 텍스트
    public Button joinButton;
    public GameObject playerStoragePre;

    // 기획팀 사운드 작업본(매칭 효과음)
    public AudioClip matchingCancleSound;
    [SerializeField]
    private AudioSource matchingAudio;

    private void Awake()
    {
        // 게임이 끝나고 로비로 돌아 올떄를 대비해서 이미 연결이 되어있는지 판단한다.
        if (PhotonNetwork.IsConnected) // 연결이 되어있으면 true, 그렇지 않으면 false
        {
            isMasterServerConnect = true;
        }
        else
        {
            isMasterServerConnect = false;
        }

        // 매칭에 연결된 모든 플레이어들을 씬으로 같이 이동하기 위함.
        PhotonNetwork.AutomaticallySyncScene = true;

        //matChingObj.SetActive(false);
    }


    private void Start()
    {
        // 포톤 네트워크에 게임 버전을 명시해주어야함
        PhotonNetwork.GameVersion = gameVersion;

        joinButton.interactable = false;

        // 서버에 접속되어 있지 않은 경우 접속을 시도함
        if (isMasterServerConnect == false)
        {
            // 마스터 서버에 접속을 시도함.
            PhotonNetwork.ConnectUsingSettings();

            connectionInfoText.text = "Connecting To Master Server...";
        }
        else if (isMasterServerConnect == true)
        {
            // 서버에 접속이 되어있다면
            OnConnectedToMaster();
        }
        
    }

    // 마스터 서버에 접속 시도 / 자동실행
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected to Master Server";
        Instantiate(playerStoragePre, Vector3.zero, Quaternion.identity);
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
    /*public void Connect()
    {
        TrojanHorse tro = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();
        tro.PlayerTrojanInfo();

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
    }*/

    // join button을 눌렀을때 메소드
    public void JoinRandomOrCreateRoom()
    {
        //print($"{nick} 랜덤 매칭 시작.");
        //PhotonNetwork.LocalPlayer.NickName = nick; // 현재 플레이어 닉네임 설정하기.

        TrojanHorse tro = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();
        tro.PlayerTrojanInfo();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomMaxPlayers; // 인원 지정.
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "maxTime", maxTime } }; // 게임 시간 지정.
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "maxTime" }; // 여기에 키 값을 등록해야, 필터링이 가능하다.

        // 방 참가를 시도하고, 실패하면 생성해서 참가함.
        connectionInfoText.text = "Connecting to Random Room...";
        PhotonNetwork.JoinRandomOrCreateRoom(
            expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "maxTime", maxTime } }, expectedMaxPlayers: (byte)roomMaxPlayers, // 참가할 때의 기준.
            roomOptions: roomOptions // 생성할 때의 기준.
        );
    }

    // 매칭 메소드
    public void CancelMatching()
    {
        // 매칭 취소
        matChingObj.SetActive(false);
        // 방을 떠남
        PhotonNetwork.LeaveRoom();

        // 기획팀 사운드 작업본(매칭 효과음)
        matchingAudio.clip = matchingCancleSound;
        matchingAudio.Play();
    }

    // 매칭 인원수 텍스트
    private void UpdatePlayerCounts()
    {
        matChingObj.gameObject.transform.GetChild(0).GetComponent<Text>().text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
    }

    /*public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "There is no empty room, Creating new Room.";

        // 네트워크 상에서 인원2인 방을 자동으로 생성함.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }*/

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with Room.";

        UpdatePlayerCounts();
        matChingObj.SetActive(true);
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName}은 인원수 {PhotonNetwork.CurrentRoom.MaxPlayers} 매칭 기다리는 중.");
        UpdatePlayerCounts();
    }

    // 룸에 들어 감.
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        /*TrojanHorse tro = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();
        tro.PlayerTrojanInfo();*/

        Debug.Log($"플레이어 {newPlayer.NickName} 방 참가.");
        UpdatePlayerCounts();

        if (PhotonNetwork.IsMasterClient)
        {
            // 목표 인원 수 채웠으면, 맵 이동을 한다. 권한은 마스터 클라이언트만.
            // PhotonNetwork.AutomaticallySyncScene = true; 를 해줬어야 방에 접속한 인원이 모두 이동함.
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.LoadLevel("Loading");
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log($"플레이어 {otherPlayer.NickName} 방 나감.");
        UpdatePlayerCounts();
    }

    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);
            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }
}