//#define 캐릭터선택_VER_1
#define 캐릭터선택_DEFAULT

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

    // TODO : 생성할 플레이어 프리팹 정보를 캐릭터 선택단계에서 가져오기
    public Dictionary<string, GameObject> mySelect = new Dictionary<string, GameObject>();
    public List<GameObject> playerPrefab; // 생성할 플레이어의 원형 프리팹

    [Header("Nexus")]
    [SerializeField]
    private GameObject[] NexusPrefab = new GameObject[2];

    [Header("Boss")]
    [SerializeField]
    private GameObject BossPrefeb;

    // turret.cs, player.cs에서 onEnable하자마자 담겨질 리스트.
    public List<GameObject> CurrentTurrets = new List<GameObject>(8);// 각 월드에서 생성된 모든 터렛들.
    public List<GameObject> CurrentPlayers = new List<GameObject>(2); // 각 월드에서 생성된 모든 플레이어들.
    //public List<GameObject> CurrentMinions = new List<GameObject>(); // 각 월드에서 생성된 모든 미니언들.

    // 플레이어 미니맵에 띄우기
    public GameObject CharacterCircle;

    public bool isGameEnd;
    public string winner;

    public TrojanHorse myData;

    private void Awake()
    {
        myData = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();
        SpawnNexus();
        SpawnPlayer();
        // buffManager 인스턴스생성 속도 맞추기 위해서 invoke사용
        Invoke(nameof(SpawnTower), 0.5f);
    }


    private void Start()
    {
        // HSW : 11 - 08 병합후 충돌로 임시 주석처리


        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            gameObject.tag = "Blue";
        }

        else
        {
            gameObject.tag = "Red";
        }
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

        mySelect.Add("Warrior", playerPrefab[0]);
        mySelect.Add("Wizard", playerPrefab[1]);
        
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        // 플레이어 생성 HSW
        GameObject player;
#if 캐릭터선택_DEFAULT
        player = PhotonNetwork.Instantiate(playerPrefab[0].name, spawnPosition.position, Quaternion.identity);
#endif

#if 캐릭터선택_VER_1
        player = PhotonNetwork.Instantiate(mySelect[myData.selectCharacter].name, spawnPosition.position, Quaternion.identity);
#endif
        // HSW


        // 미니맵 플레이어 캔버스 생성
        GameObject circle = PhotonNetwork.Instantiate(CharacterCircle.name, new Vector3(player.transform.position.x, player.transform.position.y + 30, player.transform.position.z), Quaternion.identity);

        if (PhotonNetwork.LocalPlayer.ActorNumber >= 1)
        {
            Debug.Log("이벤트 적용");
        }
    }

    int count;

    // TODO : 최적화, 파인드 오브젝트를 안쓰고 객체를 참조할수있는 방법은 없는지? 
    private void SpawnTower()
    {
        count = myData.cardId.Count;
        if (count == 0)
        {
            return;
        }

        Debug.Log($"{gameObject.tag}, 호출");
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            GameObject tower = myData.cardPrefab[0];
            int slotIndex = myData.cardIndex[0] - 4;
            GameObject newTower = PhotonNetwork.Instantiate(tower.name, tiles[slotIndex].position, Quaternion.identity);
            CheckandApplyBuffs(newTower);

        }
        else
        {
            GameObject tower = myData.cardPrefab[0];
            int slotIndex = myData.cardIndex[0] - 4;
            GameObject newTower = PhotonNetwork.Instantiate(tower.name, tiles[slotIndex + 4].position, Quaternion.identity);
            CheckandApplyBuffs(newTower);
        }
    }

    int idx = 1;
    public void UnlockTower(string tag, int level)
    {
        if (count == 0 || idx == count)
        {
            return;
        }

        if (gameObject.tag != tag)
        {
            return;
        }

        // 자기 자신 기준 1 2 3만 호출
        if (level == 3 || level == 5 || level == 7)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                GameObject tower = myData.cardPrefab[idx];
                int slotIndex = myData.cardIndex[idx] - 4;
                GameObject newTower = PhotonNetwork.Instantiate(tower.name, tiles[slotIndex].position, Quaternion.identity);
                CheckandApplyBuffs(newTower);
            }
            else
            {
                GameObject tower = myData.cardPrefab[idx];
                int slotIndex = myData.cardIndex[idx] - 4;
                GameObject newTower = PhotonNetwork.Instantiate(tower.name, tiles[slotIndex + 4].position, Quaternion.identity);
                CheckandApplyBuffs(newTower);
            }
            idx++;
        }

    }

    //private void SpawnNexus()

    private void CheckandApplyBuffs(GameObject tower)
    {
        TowerData towerData = tower.GetComponent<Turret>().towerData;
        if (towerData.TowerType == Tower_Type.Buff_Tower || towerData.TowerType == Tower_Type.DeBuff_Tower)
        {
            BuffManager.Instance.AddBuff((BuffData)towerData.Scriptables[0]);
        }
    }



        //if (PhotonNetwork.LocalPlayer.ActorNumber == 1) // blue
        //{

        //    PhotonNetwork.Instantiate(NexusPrefab[0].name, spawnPositions[2].position, Quaternion.Euler(transform.position));


        //}
    private void SpawnNexus()
    {
        Debug.Log("됨?");
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1) // blue
        {
            Debug.Log("됨?1");
            PhotonNetwork.Instantiate(NexusPrefab[0].name, spawnPositions[2].position, Quaternion.Euler(transform.position));
        }

        else // red
        {

            PhotonNetwork.Instantiate(NexusPrefab[1].name, spawnPositions[3].position, Quaternion.Euler(transform.position));
        }
    }


    // TODO : 로비로 돌아갔을떄 스킬 데이터부분 초기화 필요함

    // 중립몬스터 생성
    public void bossMonsterSpawn()
    {
        if (PhotonNetwork.IsMasterClient && PlayerHUD.Instance.BossMonsterSpawnON)
        {


          PhotonNetwork.Instantiate(BossPrefeb.name,new Vector3(0,-1f,0f), Quaternion.identity);
        }


    }

}




