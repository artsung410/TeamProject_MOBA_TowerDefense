using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################


public enum Tower
{
    GuardTower = 4,
    CannonTower,
    MinionTower,
    BuffTower
}

public enum Skill
{
    Sword = 8,
    TakeDown,
    WheelWind,
    Wield
}

public class GameManager : MonoBehaviourPunCallbacks
{
    public static event Action<int> onHpEvent = delegate { };
    public event Action onPlayerEvnet = delegate { };

    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    public Tower tower;
    public Skill skill;

    public ItemDataBaseList itemDB;
    public Transform[] tiles;
    private static GameManager instance;
    public Transform[] spawnPositions; // 플레이어가 생성할 위치
    public GameObject playerPrefab; // 생성할 플레이어의 원형 프리팹
    public GameObject[] EnemyPrefabs;
    private Transform[] minionTowerPos = new Transform[2];

    // turret.cs, player.cs에서 onEnable하자마자 담겨질 리스트.
    public List<GameObject> CurrentTurrets = new List<GameObject>();// 각 월드에서 생성된 모든 터렛들.
    public List<GameObject> CurrentPlayers = new List<GameObject>(); // 각 월드에서 생성된 모든 플레이어들.
    public List<GameObject> CurrentMinions = new List<GameObject>(); // 각 월드에서 생성된 모든 미니언들.

    // 플레이어 미니맵에 띄우기
    public GameObject CharacterCircle;
    public GameObject MinionCircle;

    public GameObject specialPFs;

    private void Start()
    {
        
        SpawnPlayer();
        SpawnTower();
        SpawnEnemy();
        SapwnSpecial();
    
    }
    float elaspedTime;
    float minionSpawnTime = 20f;
    private void Update()
    {
        elaspedTime += Time.deltaTime;
        if (elaspedTime >= minionSpawnTime)
        {
            elaspedTime = 0;
            SpawnEnemy();
            SapwnSpecial();

        }
    }

    // 플레이어 생성
    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        if (localPlayerIndex > 1)
        {
            OnLeftRoom();
        }

        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];


        // 플레이어 생성
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.identity);


        // 미니맵 플레이어 캔버스 생성
        GameObject circle = PhotonNetwork.Instantiate(CharacterCircle.name, new Vector3(player.transform.position.x, player.transform.position.y + 30, player.transform.position.z), Quaternion.identity);

        if (PhotonNetwork.LocalPlayer.ActorNumber >= 1)
        {
            Debug.Log("이벤트 적용");
        }
    }

    // 타워 생성


    private void SpawnTower()
    {
        int count = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardId.Count;
        if (count == 0)
        {
            return;
        }

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[i];
                PhotonNetwork.Instantiate(tower.name, tiles[i].position, Quaternion.identity);
                if (tower.GetComponent<Turret_LaserRange>() != null)
                {

                    minionTowerPos[0] = tiles[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[i];
                PhotonNetwork.Instantiate(tower.name, tiles[i + 4].position, Quaternion.identity);

                if (tower.GetComponent<Turret_LaserRange>() != null)
                {
                    minionTowerPos[1] = tiles[i + 4];
                }
            }
        }
    }

    // 미니언 생성
    private void SpawnEnemy()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            GameObject NomalMinion = PhotonNetwork.Instantiate(EnemyPrefabs[0].name, spawnPositions[0].position, Quaternion.identity);

            GameObject ShotMinion = PhotonNetwork.Instantiate(EnemyPrefabs[1].name, spawnPositions[0].position, Quaternion.identity);
        }
        else
        {

            //GameObject NomalMinion1 = PhotonNetwork.Instantiate(EnemyPrefabs[2].name, spawnPositions[1].position, Quaternion.identity);

            GameObject shotMinion1 = PhotonNetwork.Instantiate(EnemyPrefabs[3].name, spawnPositions[1].position, Quaternion.identity);
        }
    }

    private void SapwnSpecial()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
             
            GameObject specialminionBlue = PhotonNetwork.Instantiate(specialPFs.name, minionTowerPos[0].transform.position, Quaternion.identity);

        }
        else
        {
            GameObject specialminionRed = PhotonNetwork.Instantiate(specialPFs.name, minionTowerPos[1].transform.position, Quaternion.identity);


        }

    }

}
