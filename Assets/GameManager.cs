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
    GuardTower = 1,
    CannonTower,
    MinionTower,
    BuffTower
}

public enum Skill
{
    Sword = 5,
    TakeDown,
    WheelWind,
    Wield
}

public class GameManager : MonoBehaviourPunCallbacks
{
    public static event Action onGameEndEvent = delegate { };
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
    
    [Header("Nexus")]
    [SerializeField]
    private GameObject[] NexusPrefab = new GameObject[2];

    // turret.cs, player.cs에서 onEnable하자마자 담겨질 리스트.
    public List<GameObject> CurrentTurrets = new List<GameObject>(8);// 각 월드에서 생성된 모든 터렛들.
    public List<GameObject> CurrentPlayers = new List<GameObject>(2); // 각 월드에서 생성된 모든 플레이어들.
    //public List<GameObject> CurrentMinions = new List<GameObject>(); // 각 월드에서 생성된 모든 미니언들.

    // 플레이어 미니맵에 띄우기
    public GameObject CharacterCircle;

    public bool isGameEnd;
    public string winner;

    private void Awake()
    {
        SpawnPlayer();
    }

    private void Start()
    {
        SpawnTower();
        SpawnNexus();
    }

    float elaspedTime;
    float minionSpawnTime = 20f;
    private void Update()
    {
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
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
            //for (int i = 0; i < count; i++)
            //{
            //    GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[i];
            //    int slotIndex = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardIndex[i] - 4;
            //    //GameObject newTower = PhotonNetwork.Instantiate(tower.name, tiles[slotIndex].position, Quaternion.identity);
            //    //photonView.RPC("setItemToTower", RpcTarget.All, newTower, i);
            //}
            GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[0];
            int slotIndex = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardIndex[0] - 4;
            GameObject newTower = PhotonNetwork.Instantiate(tower.name, tiles[slotIndex].position, Quaternion.identity);
        }
        else
        {
            //for (int i = 0; i < count; i++)
            //{
            //    GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[i];
            //    int slotIndex = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardIndex[i] - 4;
            //    GameObject newTower = PhotonNetwork.Instantiate(tower.name, tiles[slotIndex + 4].position, Quaternion.identity);
            //    photonView.RPC("setItemToTower", RpcTarget.All, newTower, i);

            //}
            GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[0];
            int slotIndex = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardIndex[0] - 4;
            GameObject newTower = PhotonNetwork.Instantiate(tower.name, tiles[slotIndex + 4].position, Quaternion.identity);
        }
    }

    int idx = 1;
    public void UnlockTower(int level)
    {
        int count = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardId.Count;
        if (count == 0 || idx == count)
        {
            return;
        }
        // 자기 자신 기준 1 2 3만 호출
        if (level == 3 || level == 5 || level == 7)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[idx];
                int slotIndex = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardIndex[idx] - 4;
                GameObject newTower = PhotonNetwork.Instantiate(tower.name, tiles[slotIndex].position, Quaternion.identity);
            }
            else
            {
                GameObject tower = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardPrefab[idx];
                int slotIndex = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().cardIndex[idx] - 4;
                GameObject newTower = PhotonNetwork.Instantiate(tower.name, tiles[slotIndex + 4].position, Quaternion.identity);
            }

            idx++;
        }
        
    }

  private void SpawnNexus()
    {

        Debug.Log("됨?");
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1) // blue
        {
            Debug.Log("됨?1");
            PhotonNetwork.Instantiate(NexusPrefab[0].name,spawnPositions[2].position,Quaternion.Euler(transform.position)); 
            

        }else // red
        {
            Debug.Log("됨?2");
            PhotonNetwork.Instantiate(NexusPrefab[1].name, spawnPositions[3].position, Quaternion.Euler(transform.position));
        }
    }


}
