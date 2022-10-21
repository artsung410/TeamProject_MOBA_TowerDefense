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

    public Transform[] tiles;
    private static GameManager instance;
    public Transform[] spawnPositions; // 플레이어가 생성할 위치
    public GameObject playerPrefab; // 생성할 플레이어의 원형 프리팹
    public GameObject[] EnemyPrefabs;
    public List<GameObject> CurrentTowers;
    public int localPlayerIndex;

    private void Start()
    {
        SpawnPlayer();
        SpawnTower();
        SpawnEnemy();
    }

    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        if (localPlayerIndex > 1)
        { 
            OnLeftRoom();
        }

        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.identity);
    }

    private void SpawnTower()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[i];
                GameObject currentTower = PhotonNetwork.Instantiate(tower.name, tiles[i].position, Quaternion.identity);
                //currentTower.layer = 10;

            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[i];
                GameObject currentTower = PhotonNetwork.Instantiate(tower.name, tiles[i + 4].position, Quaternion.identity);
                //photonView.RPC("RPCUpdateLayer", RpcTarget.All, currentTower, 11);
            }
        }
    }

    private void SpawnEnemy()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            for(int i = 0; i < 1; i++)
            {
                GameObject NomalMinion = PhotonNetwork.Instantiate(EnemyPrefabs[0].name,spawnPositions[0].position,Quaternion.identity);
                GameObject ShotMinion = PhotonNetwork.Instantiate(EnemyPrefabs[1].name, spawnPositions[0].position, Quaternion.identity);
                GameObject NomalMinion1 = PhotonNetwork.Instantiate(EnemyPrefabs[2].name, spawnPositions[1].position, Quaternion.identity);
                GameObject shotMinion1 = PhotonNetwork.Instantiate(EnemyPrefabs[3].name, spawnPositions[1].position, Quaternion.identity);
            }
        }
    }

    //private void Update()
    //{
    //}

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

    [PunRPC]
    private void RPCUpdateLayer(GameObject tower, int layer)
    {
        tower.layer = 10;
    }
}