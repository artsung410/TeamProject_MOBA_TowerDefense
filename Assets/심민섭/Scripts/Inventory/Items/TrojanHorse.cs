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

    private ItemOnObject itemOnObject;

    private GameObject EquipmentItemInventory;

    public void PlayerTrojanInfo()
    {
        // PlayerNumber 받기
        playerNumber = GetComponent<PlayerStorage>().playerNumber;

        //ItemOnObject에서 가져온다.

        // 장착 슬롯 오브젝트 가져오기
        EquipmentItemInventory = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetChild(1).gameObject;

        // 하위 오브젝트들을 리스트에 넣는다.
        List<GameObject> TrojanDataList= new List<GameObject>();
        for (int i = 0; i < EquipmentItemInventory.transform.childCount; i++)
        {
            TrojanDataList.Add(EquipmentItemInventory.transform.GetChild(i).gameObject);
            //Debug.Log(TrojanDataList[i].name);
        }

        for (int i = 0; i < TrojanDataList.Count; i++)
        {
            // count가 1이면 인덱스 저장
            //Debug.Log(TrojanDataList[i].transform.childCount);
            if (TrojanDataList[i].transform.childCount == 1)
            {
                cardIndex.Add(i);
                // 그리고 데이터를 가져옴
                ItemOnObject itemOnObject = TrojanDataList[i].gameObject.transform.GetChild(0).GetComponent<ItemOnObject>();
                cardId.Add(itemOnObject.item.itemID);
                cardName.Add(itemOnObject.item.itemName);
                cardPrefab.Add(itemOnObject.item.itemModel);
            }
        }
        // 오브젝트 가져와서 하위 ItemOnObject
        //itemOnObject = Get
    }
}
