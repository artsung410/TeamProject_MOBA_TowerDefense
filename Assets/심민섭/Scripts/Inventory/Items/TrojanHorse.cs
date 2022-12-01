using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrojanHorse: MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 플레이어 넘버 : ActorNumber
    // 팀 색상 : 블루, 레드
    // 직업 : 암살자, 전사, 마법사
    // 플레이어 스폰 유무
    // 타워 스폰 유무
    // 미니언 정보


    [Header("제한 시간")]
    public int limitedTime;

    [Header("플레이어 번호 (마스터 서버 들어온 순서)")]
    public int playerNumber;

    [Header("카드 고유 ID")]
    public List<int> cardId = new List<int>();

    [Header("장착된 카드 인덱스")]
    public List<int> cardIndex = new List<int>();

    [Header("장착한 아이템")]
    public List<Item> towerItems = new List<Item>();


    private GameObject EquipmentItemInventory;

    // ----------- 승완이 에게 보낼 추가 정보---------------
    [Header("선택한 직업")]
    public string selectCharacter;

    [Header("스킬 카드 고유 ID")]
    [SerializeField]
    public List<int> skillId = new List<int>();

    [Header("스킬 장착된 카드 인덱스")]
    [SerializeField]
    public List<int> skillIndex = new List<int>();

    [Header("장착한 스킬아이템")]
    public List<Item> skillItems = new List<Item>();


    // 공격력, 사거리, 쿨타임
    [SerializeField]
    private ItemDataBaseList itemDatabase;

    public void PlayerTrojanInfo()
    {
        skillIndex.Clear();
        skillId.Clear();
        skillItems.Clear();

        cardIndex.Clear();
        cardId.Clear();
        towerItems.Clear();

        // PlayerNumber 받기
        playerNumber = GetComponent<PlayerStorage>().playerNumber;

        //ItemOnObject에서 가져온다.

        // 장착 슬롯 오브젝트 가져오기
        EquipmentItemInventory = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).GetChild(3).GetChild(1).GetChild(1).GetChild(1).GetChild(1).gameObject;

        // 하위 오브젝트들을 리스트에 넣는다.
        List<GameObject> TrojanDataList= new List<GameObject>();
        for (int i = 0; i < EquipmentItemInventory.transform.childCount; i++)
        {
            TrojanDataList.Add(EquipmentItemInventory.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < TrojanDataList.Count; i++)
        {
            // count가 1이면 인덱스 저장(아이템이 들어 있다는 것)
            if (TrojanDataList[i].transform.childCount == 1)
            {
                if (i <= 3) // 0, 1, 2, 3
                {
                    skillIndex.Add(i);
                    // 그리고 데이터를 가져옴
                    ItemOnObject itemOnObject = TrojanDataList[i].gameObject.transform.GetChild(0).GetComponent<ItemOnObject>();
                    skillId.Add(itemOnObject.item.itemID);
                    skillItems.Add(itemOnObject.item);
                }

                if (i > 3) // 4, 5, 6, 7
                {
                    cardIndex.Add(i);
                    // 그리고 데이터를 가져옴
                    ItemOnObject itemOnObject = TrojanDataList[i].gameObject.transform.GetChild(0).GetComponent<ItemOnObject>();
                    cardId.Add(itemOnObject.item.itemID);
                    towerItems.Add(itemOnObject.item);
                }
            }
        }
    }
}
