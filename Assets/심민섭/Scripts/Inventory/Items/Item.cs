using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string itemName;                 // 아이템 이름
    public int itemID;                          // 아이템의 만들어 진 순서 (Index)
    public string itemDesc;                   // 아이템 설명
    public Sprite itemIcon;                   // 아이템 이미지
    public GameObject itemModel;        // 아이템 프리펩
    public int itemValue = 1;                // 가지고 있는 아이템 갯수
    public ItemType itemType;              // 아이템이 무슨 타입인지 정의
    public float itemWeight;                 // 아이템의 무게
    public int maxStack = 5;                 // 최대 소지 갯수
    public int indexItemInList = 99;        // 인덱스
    public int rarity;                            // 아이템 얻을 확률

    // 아이템의 효과
    [SerializeField]
    public List<ItemAttribute> itemAttributes = new List<ItemAttribute>();    
    
    public Item(){}

    public Item(string name, int id, string desc, Sprite icon, GameObject model, int maxStack, ItemType type, string sendmessagetext, List<ItemAttribute> itemAttributes)                 //function to create a instance of the Item
    {
        itemName = name;
        itemID = id;
        itemDesc = desc;
        itemIcon = icon;
        itemModel = model;
        itemType = type;
        this.maxStack = maxStack;
        this.itemAttributes = itemAttributes;
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


