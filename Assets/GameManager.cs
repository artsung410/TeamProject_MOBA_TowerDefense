using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    public GameObject[] tiles;
    public GameObject[] towerPrefabs;

    private static GameManager instance;
    public Transform[] spawnPositions; // 플레이어가 생성할 위치
    public GameObject playerPrefab; // 생성할 플레이어의 원형 프리팹

    public Transform[] MinionPosition;


    public int localPlayerIndex;

    private void Start()
    {
        SpawnPlayer();

        if (photonView.IsMine)
        {
            //SpawnTower();
        }
    }

    private void SpawnTower()
    {
        tiles[1].GetComponent<Tile>().BuildTower();
    }

    private void SpawnPlayer()
    {
        // 현재 방에 들어온 로컬 플레이어의 나 자신의 번호를 가져온다.
        localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        // a플레이어 세상에서 a플레이어를 생성함, 그다음에 b c d 세상에 a의 복제본이 생성됨.
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.identity);
    }

    
   


    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    private void Update()
    {
    }

    //public void AddScore(int playerNumber, int score)
    //{
    //    playerScores[playerNumber - 1] += score;

    //    // RpcTarget : 어떤 클라이언트에게 동기화를 징행할 것인지, All이면 모든 클라이언트들에게 동기화 진행.
    //    photonView.RPC("RPCUpdateScoreText", RpcTarget.All, playerScores[0].ToString(), playerScores[1].ToString());
    //}


    //[PunRPC]
    //private void RPCUpdateScoreText(string player1ScoreText, string player2ScoreText)
    //{
    //    scoreText.text = $"{player1ScoreText} : {player2ScoreText}";
    //}

}

