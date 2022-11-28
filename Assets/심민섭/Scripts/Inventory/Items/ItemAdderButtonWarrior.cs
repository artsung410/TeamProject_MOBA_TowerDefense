using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class ItemAdderButtonWarrior : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private Inventory inv;

    private int itemID;
    private int itemValue = 1;


    [SerializeField]
    private GameObject warriorDropFieldData;

    private void Start()
    {
        inv = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<Inventory>();
    }

    public void ItemAdder()
    {
        ItemDataBaseList inventoryItemList_skill = (ItemDataBaseList)Resources.Load("WarriorSkillDatabase");
        for (int i = 0; i < inventoryItemList_skill.itemList.Count; i++)
        {
            if (warriorDropFieldData.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == inventoryItemList_skill.itemList[i].itemName)
            {
                itemID = inventoryItemList_skill.itemList[i].itemID;
                break;
            }
        }

        inv.addItemToInventory(itemID, itemValue);
        inv.stackableSettings();
        inv.OnUpdateItemList();
    }
}
