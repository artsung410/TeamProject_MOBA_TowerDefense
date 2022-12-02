using System.Collections;
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


    // card back image
    [SerializeField]
    private Sprite cardBack_warrior;
    [SerializeField]
    private Sprite cardBack_wizard;
    [SerializeField]
    private Sprite cardBack_assassin;
    [SerializeField]
    private Sprite cardBack_common;
    [SerializeField]
    private Sprite cardBack_attackTower;
    [SerializeField]
    private Sprite cardBack_minionTower;
    [SerializeField]
    private Sprite cardBack_buffTower;
    [SerializeField]
    private Sprite cardBack_randomTower;

    // 타워 카드 선택
    private string selectTowerCard;

    public void SkillResultSelect()
    {
        // 인벤토리 지정
        DrawManager.instance.SelectInventory();        

        // 여기서 어떤 카드인지 카드 데이터베이스를 지정해준다.(어떤 카드를 오픈하는지 전달해주면됨)
        if (DrawManager.instance.boxName == "Warrior Skill" || DrawManager.instance.boxName == "Warrior Skill_P")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("WarriorSkillDatabase");
            if (DrawManager.instance.boxName == "Warrior Skill")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].skillData.Probability;
                }
            }
            else if (DrawManager.instance.boxName == "Warrior Skill_P")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].skillData.Probability;
                }
            }
        }
        else if (DrawManager.instance.boxName == "Wizard Skill" || DrawManager.instance.boxName == "Wizard Skill_P")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("WizardSkillDatabase");
            if (DrawManager.instance.boxName == "Wizard Skill")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].skillData.Probability;
                }
            }
            else if (DrawManager.instance.boxName == "Wizard Skill_P")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].skillData.Probability;
                }
            }
        }
        else if (DrawManager.instance.boxName == "Common Skill" || DrawManager.instance.boxName == "Common Skill_P")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("InherenceSkillDatabase");
            if (DrawManager.instance.boxName == "Common Skill")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].skillData.Probability;
                }
            }
            else if (DrawManager.instance.boxName == "Common Skill_P")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].skillData.Probability;
                }
            }
        }
        else if (DrawManager.instance.boxName == "Attack Tower" || DrawManager.instance.boxName == "Attack Tower_P")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("AttackTowerDatabase");
            if (DrawManager.instance.boxName == "Attack Tower")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].towerData.Normal_Attack_Draw_Probability;
                }
                selectTowerCard = "Normal_Attack_Draw_Probability";
            }
            else if (DrawManager.instance.boxName == "Attack Tower_P")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].towerData.Premium_Attack_Draw_Probability;
                }
                selectTowerCard = "Premium_Attack_Draw_Probability";
            }
        }
        else if (DrawManager.instance.boxName == "Minion Tower" || DrawManager.instance.boxName == "Minion Tower_P")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("MinionTowerDatabase");
            if (DrawManager.instance.boxName == "Minion Tower")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].towerData.Normal_Minion_Draw_Probability;
                }
                selectTowerCard = "Normal_Minion_Draw_Probability";
            }
            else if (DrawManager.instance.boxName == "Minion Tower_P")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].towerData.Premium_Minion_Draw_Probability;
                }
                selectTowerCard = "Premium_Minion_Draw_Probability";
            }
        }
        else if (DrawManager.instance.boxName == "Buff & Debuff Tower" || DrawManager.instance.boxName == "Buff & Debuff Tower_P")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("BuffTowerDatabase");
            if (DrawManager.instance.boxName == "Buff & Debuff Tower")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].towerData.Normal_Buff_Debuff_Draw_Probability;
                }
                selectTowerCard = "Normal_Buff_Debuff_Draw_Probability";
            }
            else if (DrawManager.instance.boxName == "Buff & Debuff Tower_P")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].towerData.Premium_Buff_Debuff_Draw_Probability;
                }
                selectTowerCard = "Premium_Buff_Debuff_Draw_Probability";
            }
        }
        else if (DrawManager.instance.boxName == "Random Tower" || DrawManager.instance.boxName == "Random Tower_P")
        {
            inventoryItemList_skill = (ItemDataBaseList)Resources.Load("TowerDatabase");
            if (DrawManager.instance.boxName == "Random Tower")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].towerData.Normal_Random_Draw_Probability;
                }
                selectTowerCard = "Normal_Random_Draw_Probability";
            }
            else if (DrawManager.instance.boxName == "Random Tower_P")
            {
                total = 0;
                for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
                {
                    // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
                    total += inventoryItemList_skill.itemList[i].towerData.Premium_Random_Draw_Probability;
                }
                selectTowerCard = "Premium_Random_Draw_Probability";
            }
        }
        else
        {
            Debug.LogError("맞는 데이터베이스가 없습니다.");
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
            if (result[i].ClassType == "Warrior") // 타워 1,2,4 없으면 랜덤
            {
                cardUI.CardUISet(result[i].itemIcon, cardBack_warrior, result[i].itemName, result[i].itemDesc);
            }
            else if (result[i].ClassType == "Wizard")
            {
                cardUI.CardUISet(result[i].itemIcon, cardBack_wizard, result[i].itemName, result[i].itemDesc);
            }
            else if (result[i].ClassType == "Assassin")
            {
                cardUI.CardUISet(result[i].itemIcon, cardBack_assassin, result[i].itemName, result[i].itemDesc);
            }
            else if (result[i].ClassType == "Inherence")
            {
                cardUI.CardUISet(result[i].itemIcon, cardBack_common, result[i].itemName, result[i].itemDesc);
            } // 오픈하는 덱에 따라 백이미지를 선택한다.
            else if (DrawManager.instance.boxName == "Attack Tower" || DrawManager.instance.boxName == "Attack Tower_P") // 공격 타워
            {
                cardUI.CardUISet(result[i].itemIcon, cardBack_attackTower, result[i].towerData.NickName, result[i].itemDesc);
            }
            else if (DrawManager.instance.boxName == "Minion Tower" || DrawManager.instance.boxName == "Minion Tower_P") // 버프타워 3, 디버프 타워
            {
                cardUI.CardUISet(result[i].itemIcon, cardBack_minionTower, result[i].towerData.NickName, result[i].itemDesc);
            }
            else if (DrawManager.instance.boxName == "Buff & Debuff Tower" || DrawManager.instance.boxName == "Buff & Debuff Tower_P") // 미니언 타워
            {
                cardUI.CardUISet(result[i].itemIcon, cardBack_buffTower, result[i].towerData.NickName, result[i].itemDesc);
            }
            else if (DrawManager.instance.boxName == "Random Tower" || DrawManager.instance.boxName == "Random Tower_P") // 랜덤 타워
            {
                cardUI.CardUISet(result[i].itemIcon, cardBack_randomTower, result[i].towerData.NickName, result[i].itemDesc);
            }
        }
        // 뽑은 아이템을 getDrawResultItem에 모두 넣고 실행한다.
        DrawManager.instance.ItemProduceAndIventoryMove();

    }
    // 가중치 랜덤의 설명은 영상을 참고.
    public int SkillRandomCard()
    {
        float weight = 0;
        float selectNum = 0;

        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));
        //Debug.Log(selectNum);
        for (int i = 1; i <= inventoryItemList_skill.itemList.Count - 1; i++)
        {
            if (inventoryItemList_skill.itemList[i].itemType == ItemType.Skill)
                weight += inventoryItemList_skill.itemList[i].skillData.Probability;
            if (inventoryItemList_skill.itemList[i].itemType == ItemType.Tower)
            {
                if (selectTowerCard == "Normal_Attack_Draw_Probability")
                {
                    weight += inventoryItemList_skill.itemList[i].towerData.Normal_Attack_Draw_Probability;
                }
                else if (selectTowerCard == "Premium_Attack_Draw_Probability")
                {
                    weight += inventoryItemList_skill.itemList[i].towerData.Premium_Attack_Draw_Probability;
                }
                else if (selectTowerCard == "Normal_Minion_Draw_Probability")
                {
                    weight += inventoryItemList_skill.itemList[i].towerData.Normal_Minion_Draw_Probability;
                }
                else if (selectTowerCard == "Premium_Minion_Draw_Probability")
                {
                    weight += inventoryItemList_skill.itemList[i].towerData.Premium_Minion_Draw_Probability;
                }
                else if (selectTowerCard == "Normal_Buff_Debuff_Draw_Probability")
                {
                    weight += inventoryItemList_skill.itemList[i].towerData.Normal_Buff_Debuff_Draw_Probability;
                }
                else if (selectTowerCard == "Premium_Buff_Debuff_Draw_Probability")
                {
                    weight += inventoryItemList_skill.itemList[i].towerData.Premium_Buff_Debuff_Draw_Probability;
                }
                else if (selectTowerCard == "Normal_Random_Draw_Probability")
                {
                    weight += inventoryItemList_skill.itemList[i].towerData.Normal_Random_Draw_Probability;
                }
                else if (selectTowerCard == "Premium_Random_Draw_Probability")
                {
                    weight += inventoryItemList_skill.itemList[i].towerData.Premium_Random_Draw_Probability;
                }
            }
            if (inventoryItemList_skill.itemList[i].itemType == ItemType.uniqueSkill)
                weight += inventoryItemList_skill.itemList[i].skillData.Probability;
            if (selectNum <= weight)
            {
                return i;
            }
        }
        return -1;
    }
}