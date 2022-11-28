using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemAdder : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // DropBox
    private TMP_Dropdown dropBox_warrior;
    private TMP_Dropdown dropBox_wizard;
    private TMP_Dropdown dropBox_inherence;
    private TMP_Dropdown dropBox_tower;

    // 아이템 명
    private List<string> dropOptions = new List<string>();
    // 아이템 ID
    private int itemID;


    void Start()
    {
        if (gameObject.name == "ItemAdder(warrior)")
        {
            dropBox_warrior = gameObject.GetComponent<TMP_Dropdown>();
            WarriorItemAdder();
        }
        if (gameObject.name == "ItemAdder(wizard)")
        {
            dropBox_wizard = gameObject.GetComponent<TMP_Dropdown>();
            WizardItemAdder();
        }
        if (gameObject.name == "ItemAdder(inherence)")
        {
            dropBox_inherence = gameObject.GetComponent<TMP_Dropdown>();
            InherenceItemAdder();
        }
        if (gameObject.name == "ItemAdder(tower)")
        {
            dropBox_tower = gameObject.GetComponent<TMP_Dropdown>();
            TowerItemAdder();
        }
    }

    private void WarriorItemAdder()
    {
        ItemDataBaseList inventoryItemList_skill = (ItemDataBaseList)Resources.Load("WarriorSkillDatabase");
        dropOptions.Clear();

        for (int i = 1; i < inventoryItemList_skill.itemList.Count; i++)
        {
            dropOptions.Add(inventoryItemList_skill.itemList[i].itemName);
        }
        dropBox_warrior.AddOptions(dropOptions);
    }

    private void WizardItemAdder()
    {
        ItemDataBaseList inventoryItemList_skill = (ItemDataBaseList)Resources.Load("WizardSkillDatabase");
        dropOptions.Clear();

        for (int i = 1; i < inventoryItemList_skill.itemList.Count; i++)
        {
            dropOptions.Add(inventoryItemList_skill.itemList[i].itemName);
        }

        dropBox_wizard.AddOptions(dropOptions);
    }

    private void InherenceItemAdder()
    {
        ItemDataBaseList inventoryItemList_skill = (ItemDataBaseList)Resources.Load("InherenceSkillDatabase");
        dropOptions.Clear();

        for (int i = 1; i < inventoryItemList_skill.itemList.Count; i++)
        {
            dropOptions.Add(inventoryItemList_skill.itemList[i].itemName);
        }

        dropBox_inherence.AddOptions(dropOptions);
    }

    private void TowerItemAdder()
    {
        ItemDataBaseList inventoryItemList_skill = (ItemDataBaseList)Resources.Load("TowerDatabase");
        dropOptions.Clear();

        for (int i = 1; i < inventoryItemList_skill.itemList.Count; i++)
        {
            dropOptions.Add(inventoryItemList_skill.itemList[i].itemName);
        }

        dropBox_tower.AddOptions(dropOptions);
    }

}
