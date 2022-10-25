using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrojanHorse: MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    [Header("플레이어 번호 (마스터 서버 들어온 순서)")]
    public int playerNumber;

    [Header("카드 고유 ID")]
    public List<int> cardId = new List<int>();

    [Header("장착된 카드 인덱스")]
    public List<int> cardIndex = new List<int>();

    [Header("장착한 카드 명")]
    public List<string> cardName = new List<string>();

    [Header("장착한 프리펩")]
    public List<GameObject> cardPrefab  = new List<GameObject>();

    [Header("장착한 아이템")]
    public List<Item> cardItems = new List<Item>();

    private ItemOnObject itemOnObject;

    private GameObject EquipmentItemInventory;

    // ----------- 승완이 에게 보낼 추가 정보---------------
    [Header("스킬 카드 고유 ID")]
    [SerializeField]
    private List<int> skillId = new List<int>();

    [Header("스킬 장착된 카드 인덱스")]
    [SerializeField]
    private List<int> skillIndex = new List<int>();

    [Header("스킬 장착한 카드 명")]
    [SerializeField]
    private List<string> skillCName = new List<string>();

    [Header("스킬 공격력")]
    [SerializeField]
    private List<int> skillATK = new List<int>();

    [Header("스킬 사거리")]
    [SerializeField]
    private List<int> skillCrossroad = new List<int>();

    [Header("스킬 쿨타임")]
    [SerializeField]
    private List<int> skillCoolTime = new List<int>();

    [Header("장착한 스킬아이템")]
    public List<Item> skillItems = new List<Item>();

    // 공격력, 사거리, 쿨타임
    [SerializeField]
    private ItemDataBaseList itemDatabase;

    public void PlayerTrojanInfo()
    {
        // PlayerNumber 받기
        playerNumber = GetComponent<PlayerStorage>().playerNumber;

        //ItemOnObject에서 가져온다.

        // 장착 슬롯 오브젝트 가져오기
        EquipmentItemInventory = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetChild(1).GetChild(1).gameObject;

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
                    skillCName.Add(itemOnObject.item.itemName);
                    skillItems.Add(itemOnObject.item);
                }

                if (i > 3) // 4, 5, 6, 7
                {
                    cardIndex.Add(i);
                    // 그리고 데이터를 가져옴
                    ItemOnObject itemOnObject = TrojanDataList[i].gameObject.transform.GetChild(0).GetComponent<ItemOnObject>();
                    cardId.Add(itemOnObject.item.itemID);
                    cardName.Add(itemOnObject.item.itemName);
                    cardPrefab.Add(itemOnObject.item.itemModel);
                    cardItems.Add(itemOnObject.item);
                }
            }
        }


        /*if (skillId.Count == 0)
        {
            return;
        }

        int count = TrojanDataList.Count / 2;

        for (int i = 0; i < count; i++) // 4
        {
            for (int j = 0; j < itemDatabase.itemList.Count; j++) // 100
            {
                if (skillId[i] == itemDatabase.itemList[j].itemID)
                {
                    // 정보를 가져온다. Item Attributes
                    skillATK.Add(itemDatabase.itemList[j].itemAttributes[0].attributeValue);
                    //skillCrossroad.Add(itemDatabase.itemList[j].itemAttributes[1].attributeValue);
                    skillCoolTime.Add(itemDatabase.itemList[j].itemAttributes[1].attributeValue);
                }
            }
        }*/
    }
}
