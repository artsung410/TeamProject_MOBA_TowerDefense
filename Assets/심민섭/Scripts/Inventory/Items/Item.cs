using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Master
// ###############################################
//             NAME : Simstealer                      
//             MAIL : minsub4400@gmail.com         
// ###############################################

// Modify Member
// 20221021-16:00 Item필드 추가.
// 20221103-16:00 ScriptableObject필드 추가.
// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

[System.Serializable]
public class Item
{
    public string itemName;                     // 아이템 이름
    public string ClassType;                    // 직업 타입
    public int itemID;                          // 아이템의 만들어 진 순서 (Index)
    [TextArea] public string itemDesc;          // 아이템 설명
    public Sprite itemIcon;                     // 아이템 이미지
    //public GameObject itemModel;              // 아이템 프리펩
    public string objType;                      // 오브젝트 타입
    public int itemValue = 1;                   // 가지고 있는 아이템 갯수
    public ItemType itemType;                   // 아이템이 무슨 타입인지 정의
    public float itemWeight;                    // 아이템의 무게
    public int maxStack = 99;                   // 최대 소지 갯수
    public int indexItemInList = 99;            // 인덱스
    public float rarity;                        // 아이템 얻을 확률
    public int CombinationValue;                // 조합에 필요한 카드 개수
    public TowerBlueprint towerData;            // 타워 데이터
    public PlayerSkillDatas skillData;          // 스킬 데이터

    public Item(){}

    public Item(string name, int id, string desc, Sprite icon, GameObject model, ScriptableObject data, int maxStack, ItemType type, string sendmessagetext, List<ItemAttribute> itemAttributes)                 //function to create a instance of the Item
    {
        itemName = name;
        itemID = id;
        itemDesc = desc;
        itemIcon = icon;
        //itemModel = model;
        //inGameData = data;
        itemType = type;
        this.maxStack = maxStack;
        //this.itemAttributes = itemAttributes;
    }

    /// <summary>
    /// 해당 오브젝트를 복사한다.
    /// </summary>
    /// <returns></returns>
    public Item getCopy()
    {
        // System.MemberwiseClone()는 해당 오브젝트의 복사본을 만든다.
        return (Item)this.MemberwiseClone();        
    }   
    
    
}


