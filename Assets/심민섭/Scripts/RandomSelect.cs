﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelect : MonoBehaviour
{
    //public List<Card> deck = new List<Card>();  // 카드 덱
    public float total;  // 카드들의 가중치 총 합

    //[SerializeField]
    //private ItemDataBaseList SkillDatabase;
    private ItemDataBaseList inventoryItemList_skill;
    [SerializeField]
    private ItemDataBaseList TowerDatabase;

    public List<Item> result = new List<Item>();  // 랜덤하게 선택된 카드를 담을 리스트

    public Transform parent;
    public GameObject cardprefab;

    public void SkillResultSelect()
    {
        // 인벤토리 지정
        DrawManager.instance.SelectInventory();

        // 여기서 어떤 카드인지 카드 데이터베이스를 지정해준다.(어떤 카드를 오픈하는지 전달해주면됨)
        if (DrawManager.instance.boxName == "Warrior Skill")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("WarriorSkillDatabase");
            Debug.Log("전사 스킬");
        }
        else if (DrawManager.instance.boxName == "Wizard Skill")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("WizardSkillDatabase");
            Debug.Log("마법사 스킬");
        }
        else if (DrawManager.instance.boxName == "Common Skill")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("InherenceSkillDatabase");
            Debug.Log("공통 스킬");
        }
        else if (DrawManager.instance.boxName == "Attack Tower")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("AttackTowerDatabase");
            Debug.Log("공격 타워");
        }
        else if (DrawManager.instance.boxName == "Minion Tower")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("MinionTowerDatabase");
            Debug.Log("미니언 타워");
        }
        else if (DrawManager.instance.boxName == "Buff Tower")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("BuffTowerDatabase");
            Debug.Log("버프 타워");
        }
        else if (DrawManager.instance.boxName == "Random Tower")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("TowerDatabase");
            Debug.Log("랜덤 타워");
        }
        else
        {
            Debug.LogError("맞는 데이터베이스가 없습니다.");
        }


        total = 0;
        for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
        {
            // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
            total += inventoryItemList_skill.itemList[i].rarity;
        }

        if (total == 0)
        {
            Debug.LogError("total : 0");
        }

        DrawManager.instance.getDrawResult.Clear();
        DrawManager.instance.getDrawResultItem.Clear();
        DrawManager.instance.ResultItem.Clear();
        result.Clear();
        // 정해진 갯수를 받아서 반복
        for (int i = 0; i < DrawManager.instance.boxCount; i++)
        {
            int cardIndex = 0;
            // 가중치 랜덤을 돌리면서 결과 리스트에 넣어줍니다.
            DrawManager.instance.getDrawResult.Add(cardIndex = SkillRandomCard()); // 인덱스만 넣었음
            // 카드 인덱스를 가지고 아이템 데이터 베이스에서 아이템을 찾아서 저장함.
            for (int itemdatabase = 1; itemdatabase <= inventoryItemList_skill.itemList.Count - 1; itemdatabase++)
            {
                // 인덱스가 같으면 해당 아이템을 result에 저장함
                if (DrawManager.instance.getDrawResult[i] == inventoryItemList_skill.itemList[itemdatabase].itemID)
                {
                    
                    result.Add(inventoryItemList_skill.itemList[itemdatabase]);
                    // 아이템 getDrawResultItem에 넣기 (Type : Item)
                    // Add 하기전에 리스트가 담고 있는 아이템이랑 비교하기
                    DrawManager.instance.getDrawResultItem.Add(inventoryItemList_skill.itemList[itemdatabase]);
                }
            }
            // 카드 보여주는 부분
            CardUI cardUI = Instantiate(cardprefab, parent).GetComponent<CardUI>();
            // 생성 된 카드에 결과 리스트의 정보를 넣어줍니다.
            cardUI.CardUISet(cardIndex, result[i].itemIcon, result[i].itemName);
        }
        // 뽑은 아이템을 getDrawResultItem에 모두 넣고 실행한다.
        DrawManager.instance.ItemProduceAndIventoryMove();

    }
    // 가중치 랜덤의 설명은 영상을 참고.
    public int SkillRandomCard()
    {
        float weight = 0;
        int selectNum = 0;

        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));
        //Debug.Log(selectNum);
        for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
        {
            weight += inventoryItemList_skill.itemList[i].rarity;
            if (selectNum <= weight)
            {
                // CardIndex를 뽑음
                //Debug.Log($"i : {i}");
                return i;
            }
        }
        return -1;
    }
}