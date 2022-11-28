using System.Collections.Generic;
using UnityEngine;

public struct ItemStruct
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    public string itemName;
    public string ClassType;
    public int itemID;
    public string itemDesc;
    public Sprite itemIcon;
    public GameObject itemModel;
    public ScriptableObject inGameData;
    public int itemValue;
    public ItemType itemType;
    public float itemWeight;
    public int maxStack;
    public int indexItemInList;
    public float rarity;
    public int CombinationValue;

    public TowerBlueprint towerData;
    public PlayerSkillDatas skillData;
    public List<ItemAttribute> itemAttributes;
    public List<GameObject> specialPrefabs;
    public List<BuffData> buffDatas;

}
