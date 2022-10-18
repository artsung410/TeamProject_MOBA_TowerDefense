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
    public Transform[] spawnPositions; // �÷��̾ ������ ��ġ
    public GameObject playerPrefab; // ������ �÷��̾��� ���� ������

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
        // ���� �濡 ���� ���� �÷��̾��� �� �ڽ��� ��ȣ�� �����´�.
        localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        // a�÷��̾� ���󿡼� a�÷��̾ ������, �״����� b c d ���� a�� �������� ������.
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

    //    // RpcTarget : � Ŭ���̾�Ʈ���� ����ȭ�� ¡���� ������, All�̸� ��� Ŭ���̾�Ʈ�鿡�� ����ȭ ����.
    //    photonView.RPC("RPCUpdateScoreText", RpcTarget.All, playerScores[0].ToString(), playerScores[1].ToString());
    //}


    //[PunRPC]
    //private void RPCUpdateScoreText(string player1ScoreText, string player2ScoreText)
    //{
    //    scoreText.text = $"{player1ScoreText} : {player2ScoreText}";
    //}
}