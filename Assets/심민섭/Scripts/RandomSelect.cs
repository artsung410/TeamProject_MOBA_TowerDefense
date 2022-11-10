using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelect : MonoBehaviour
{
    //public List<Card> deck = new List<Card>();  // 카드 덱
    public int total;  // 카드들의 가중치 총 합

    [SerializeField]
    private ItemDataBaseList itemDataBase;

    public List<Item> result = new List<Item>();  // 랜덤하게 선택된 카드를 담을 리스트

    public Transform parent;
    public GameObject cardprefab;

    public void ResultSelect()
    {
        total = 0;
        for (int i = 121; i <= 130; i++)
        {
            // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
            total += itemDataBase.itemList[i].rarity;
        }

        if (total == 0)
        {
            Debug.LogError("total : 0");
        }

        DrawManager.instance.getDrawResult.Clear();
        result.Clear();
        // 정해진 갯수를 받아서 반복
        for (int i = 0; i < DrawManager.instance.boxCount; i++)
        {
            int cardIndex = 0;
            // 가중치 랜덤을 돌리면서 결과 리스트에 넣어줍니다.
            DrawManager.instance.getDrawResult.Add(cardIndex = RandomCard()); // 인덱스만 넣었음
            // 카드 인덱스를 가지고 아이템 데이터 베이스에서 아이템을 찾아서 저장함.
            for (int itemdatabase = 121; itemdatabase <= 130; itemdatabase++)
            {
                // 인덱스가 같으면 해당 아이템을 result에 저장함
                if (DrawManager.instance.getDrawResult[i] == itemDataBase.itemList[itemdatabase].itemID)
                {
                    result.Add(itemDataBase.itemList[itemdatabase]);
                }
            }
            // 카드 보여주는 부분
            CardUI cardUI = Instantiate(cardprefab, parent).GetComponent<CardUI>();
            // 생성 된 카드에 결과 리스트의 정보를 넣어줍니다.
            cardUI.CardUISet(cardIndex, result[i].itemIcon, result[i].itemName);
        }

    }
    // 가중치 랜덤의 설명은 영상을 참고.
    public int RandomCard()
    {
        int weight = 0;
        int selectNum = 0;

        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));
        //Debug.Log(selectNum);
        for (int i = 121; i <= 130; i++)
        {
            weight += itemDataBase.itemList[i].rarity;
            if (selectNum <= weight)
            {
                // CardIndex를 뽑음
                Debug.Log($"i : {i}");
                return i;
            }
        }
        return -1;
    }
}