using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    public Tile[] tiles;
    private static GameManager instance;
    public Transform[] spawnPositions; // 플레이어가 생성할 위치
    public GameObject playerPrefab; // 생성할 플레이어의 원형 프리팹
    public List<GameObject> CurrentTowers;
    public int localPlayerIndex;

    private void Start()
    {
        SpawnPlayer();
        SpawnTower();
    }

    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        GameObject playerPf = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.identity);

    }

    private void SpawnTower()
    {
        if (GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().playerNumber == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[i];
                tiles[i].BuildTower(tower);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[i];
                tiles[i + 4].BuildTower(tower);
                
            }
        }
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