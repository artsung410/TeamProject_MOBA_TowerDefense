using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
    Inventory inv;

    SerializedProperty inventoryHeight;
    SerializedProperty inventoryWidth;
    SerializedProperty inventorySlotSize;
    SerializedProperty inventoryIconSize;
    SerializedProperty inventoryStackable;
    SerializedProperty mainInventory;

    SerializedProperty slotsPaddingBetweenX;
    SerializedProperty slotsPaddingBetweenY;
    SerializedProperty slotsPaddingLeft;
    SerializedProperty slotsPaddingRight;
    SerializedProperty slotsPaddingTop;
    SerializedProperty slotsPaddingBottom;
    SerializedProperty positionNumberX;
    SerializedProperty positionNumberY;


    private bool showStackableItems;


    private bool showInventorySettings = true;
    private bool showInventoryPadding = false;
    private bool showStackableItemsSettings = false;

    private int itemID;
    private int itemValue = 1;
    private int imageTypeIndex;



    void OnEnable()
    {
        inv = target as Inventory;
        inventoryHeight = serializedObject.FindProperty("height");
        inventoryWidth = serializedObject.FindProperty("width");
        inventorySlotSize = serializedObject.FindProperty("slotSize");
        inventoryIconSize = serializedObject.FindProperty("iconSize");
        slotsPaddingBetweenX = serializedObject.FindProperty("paddingBetweenX");
        slotsPaddingBetweenY = serializedObject.FindProperty("paddingBetweenY");
        slotsPaddingLeft = serializedObject.FindProperty("paddingLeft");
        slotsPaddingRight = serializedObject.FindProperty("paddingRight");
        slotsPaddingTop = serializedObject.FindProperty("paddingTop");
        slotsPaddingBottom = serializedObject.FindProperty("paddingBottom");
        inventoryStackable = serializedObject.FindProperty("stackable");
        positionNumberX = serializedObject.FindProperty("positionNumberX");
        positionNumberY = serializedObject.FindProperty("positionNumberY");
        mainInventory = serializedObject.FindProperty("mainInventory");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.BeginVertical("Box");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(mainInventory, new GUIContent("Player Inventory"));
        if (EditorGUI.EndChangeCheck())
        {
            //inv.setAsMain();
        }
        GUILayout.EndVertical();


        GUILayout.BeginVertical("Box");
        EditorGUI.indentLevel++;
        GUILayout.BeginVertical("Box");
        showInventorySettings = EditorGUILayout.Foldout(showInventorySettings, "Inventory Settings");
        if (showInventorySettings)
        {
            sizeOfInventoryGUI();
        }
        GUILayout.EndVertical();

        if (!inv.characterSystem())
        {
            GUILayout.BeginVertical("Box");
            showStackableItemsSettings = EditorGUILayout.Foldout(showStackableItemsSettings, "Stacking/Splitting");
            if (showStackableItemsSettings)
            {
                stackableItemsSettings();
                GUILayout.Space(20);
            }
            GUILayout.EndVertical();

        }
        EditorGUI.indentLevel--;
        GUILayout.EndVertical();


        serializedObject.ApplyModifiedProperties();
        SceneView.RepaintAll();
        GUILayout.BeginVertical("Box");
        addItemGUI();
        GUILayout.EndVertical();

    }


    void stackableItemsSettings()
    {
        EditorGUILayout.PropertyField(inventoryStackable, new GUIContent("Splitting/Stacking"));
        if (inventoryStackable.boolValue)
        {
            EditorGUI.indentLevel++;
            showStackableItems = EditorGUILayout.Foldout(showStackableItems, "StackNumberPosition");
            if (showStackableItems)
            {
                inventoryStackable.boolValue = true;
                EditorGUI.BeginChangeCheck();
                EditorGUI.indentLevel++;
                positionNumberX.intValue = EditorGUILayout.IntSlider("Position X:", positionNumberX.intValue, -(inventorySlotSize.intValue / 2), inventorySlotSize.intValue / 2);
                positionNumberY.intValue = EditorGUILayout.IntSlider("Position Y:", positionNumberY.intValue, -(inventorySlotSize.intValue / 2), inventorySlotSize.intValue / 2);
                EditorGUI.indentLevel--;
                if (EditorGUI.EndChangeCheck())
                {
                    inv.stackableSettings();
                }
            }
            else
            {
                inv.stackableSettings();
            }
            EditorGUI.indentLevel--;
        }
        else
        {
            inv.stackableSettings();
        }


    }

    void sizeOfInventoryGUI()
    {

        EditorGUI.indentLevel++;
        EditorGUI.BeginChangeCheck();
        //EditorGUILayout.IntSlider(inventoryHeight, 1, 10, new GUIContent("Height"));
        //EditorGUILayout.IntSlider(inventoryWidth, 1, 10, new GUIContent("Width"));
        if (EditorGUI.EndChangeCheck())
        {
            inv.setImportantVariables();
            inv.updateSlotAmount();
            //inv.adjustInventorySize();
            //inv.updatePadding(slotsPaddingBetweenX.intValue, slotsPaddingBetweenY.intValue);
            inv.updateSlotSize(inventorySlotSize.intValue);
            inv.stackableSettings();
        }

        /*EditorGUI.BeginChangeCheck();
        EditorGUILayout.IntSlider(inventorySlotSize, 20, 100, new GUIContent("Slot Size"));                         
        if (EditorGUI.EndChangeCheck())                                                                   
        {
            inv.setImportantVariables();
            inv.adjustInventorySize();
            inv.updateSlotSize(inventorySlotSize.intValue);
        }*/

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.IntSlider(inventoryIconSize, 20, 100, new GUIContent("Icon Size"));
        if (EditorGUI.EndChangeCheck())                                                     
        {
            inv.updateIconSize(inventoryIconSize.intValue);
        }

        GUILayout.BeginVertical("Box");
        showInventoryPadding = EditorGUILayout.Foldout(showInventoryPadding, "Padding");
        if (showInventoryPadding)
        {

            EditorGUI.BeginChangeCheck();
            EditorGUI.indentLevel++;
            slotsPaddingLeft.intValue = EditorGUILayout.IntField("Left:", slotsPaddingLeft.intValue);
            slotsPaddingRight.intValue = EditorGUILayout.IntField("Right:", slotsPaddingRight.intValue);
            slotsPaddingBetweenX.intValue = EditorGUILayout.IntField("Spacing X:", slotsPaddingBetweenX.intValue);
            slotsPaddingBetweenY.intValue = EditorGUILayout.IntField("Spacing Y:", slotsPaddingBetweenY.intValue);
            slotsPaddingTop.intValue = EditorGUILayout.IntField("Top:", slotsPaddingTop.intValue);
            slotsPaddingBottom.intValue = EditorGUILayout.IntField("Bottom:", slotsPaddingBottom.intValue);
            EditorGUI.indentLevel--;
            if (EditorGUI.EndChangeCheck())
            {
                //inv.adjustInventorySize();
                //inv.updatePadding(slotsPaddingBetweenX.intValue, slotsPaddingBetweenY.intValue);
            }

        }
        GUILayout.EndVertical();


        EditorGUI.indentLevel--;
    }

    // 인스펙터를 통해 인벤토리에 아이템 추가
    void addItemGUI()
    {
        if (!inv.characterSystem())
        {
            // 인스펙터의 라벨(명칭)을 Add an item으로 명명
            GUILayout.Label("Add an Skill item: (전사 스킬 추가)");

            //inv.setImportantVariables(); // 상단 GUI 요소에 공백
            EditorGUILayout.BeginHorizontal(); // 수평 GUI 요소 시작
            // 인스펙터가 가로로 생성된다.

            // 아이템 DB 오브젝트를 생성하고
            ItemDataBaseList inventoryItemList_warriorskill = (ItemDataBaseList)Resources.Load("WarriorSkillDatabase");

            // itemcount 길이의 문자열 배열 생성
            string[] items_warriorskill = new string[inventoryItemList_warriorskill.itemList.Count];
            for (int i = 1; i < items_warriorskill.Length; i++)
            {
                items_warriorskill[i] = inventoryItemList_warriorskill.itemList[i].itemName;
            }
            itemID = EditorGUILayout.Popup("", itemID, items_warriorskill, EditorStyles.popup);
            itemValue = EditorGUILayout.IntField("", itemValue, GUILayout.Width(40));
            GUI.color = Color.green;
            if (GUILayout.Button("Add Item"))
            {
                inv.addItemToInventory(itemID, itemValue);
                inv.stackableSettings();
            }
            inv.OnUpdateItemList();
            EditorGUILayout.EndHorizontal(); // 수평 GUI 레이아웃 종료

            GUI.color = Color.white;
            //-------------------------------------------------------------------------
            GUILayout.Label("Add an Skill item: (마법사 스킬 추가)");

            //inv.setImportantVariables(); // 상단 GUI 요소에 공백
            EditorGUILayout.BeginHorizontal(); // 수평 GUI 요소 시작

            // 아이템 DB 오브젝트를 생성하고
            ItemDataBaseList inventoryItemList_wizardskill = (ItemDataBaseList)Resources.Load("WizardSkillDatabase");

            // itemcount 길이의 문자열 배열 생성
            string[] items_wizardskill = new string[inventoryItemList_wizardskill.itemList.Count];
            for (int i = 1; i < items_wizardskill.Length; i++)
            {
                items_wizardskill[i] = inventoryItemList_wizardskill.itemList[i].itemName;
            }
            itemID = EditorGUILayout.Popup("", itemID, items_wizardskill, EditorStyles.popup);
            itemValue = EditorGUILayout.IntField("", itemValue, GUILayout.Width(40));
            GUI.color = Color.green;
            if (GUILayout.Button("Add Item"))
            {
                inv.addItemToInventory(itemID, itemValue);
                inv.stackableSettings();
            }
            inv.OnUpdateItemList();
            EditorGUILayout.EndHorizontal(); // 수평 GUI 레이아웃 종료

            GUI.color = Color.white;
            //-----------------------------------------------------------------------------
            GUILayout.Label("Add an Skill item: (공통 스킬 추가)");

            //inv.setImportantVariables(); // 상단 GUI 요소에 공백
            EditorGUILayout.BeginHorizontal(); // 수평 GUI 요소 시작

            // 아이템 DB 오브젝트를 생성하고
            ItemDataBaseList inventoryItemList_inherenceskill = (ItemDataBaseList)Resources.Load("InherenceSkillDatabase");

            // itemcount 길이의 문자열 배열 생성
            string[] items_inherenceskill = new string[inventoryItemList_inherenceskill.itemList.Count];
            for (int i = 1; i < items_inherenceskill.Length; i++)
            {
                items_inherenceskill[i] = inventoryItemList_inherenceskill.itemList[i].itemName;
            }
            itemID = EditorGUILayout.Popup("", itemID, items_inherenceskill, EditorStyles.popup);
            itemValue = EditorGUILayout.IntField("", itemValue, GUILayout.Width(40));
            GUI.color = Color.green;
            if (GUILayout.Button("Add Item"))
            {
                inv.addItemToInventory(itemID, itemValue);
                inv.stackableSettings();
            }
            inv.OnUpdateItemList();
            EditorGUILayout.EndHorizontal(); // 수평 GUI 레이아웃 종료

            GUI.color = Color.white;

            //---------------------------------------------------------------------------------
            GUILayout.Label("Add an Tower item: (타워는 'Panel_Inventor'에서 Add 하세요!!)");

            //inv.setImportantVariables(); // 상단 GUI 요소에 공백
            EditorGUILayout.BeginHorizontal(); // 수평 GUI 요소 시작
            // 인스펙터가 가로로 생성된다.

            // 아이템 DB 오브젝트를 생성하고
            ItemDataBaseList inventoryItemList_tower = (ItemDataBaseList)Resources.Load("TowerDatabase");

            // itemcount 길이의 문자열 배열 생성
            string[] items_tower = new string[inventoryItemList_tower.itemList.Count];
            for (int i = 1; i < items_tower.Length; i++)
            {
                items_tower[i] = inventoryItemList_tower.itemList[i].itemName;
            }
            itemID = EditorGUILayout.Popup("", itemID, items_tower, EditorStyles.popup);
            itemValue = EditorGUILayout.IntField("", itemValue, GUILayout.Width(40));
            GUI.color = Color.green;
            if (GUILayout.Button("Add Item"))
            {
                inv.addItemToInventory(itemID, itemValue);
                inv.stackableSettings();
            }
            inv.OnUpdateItemList();

            EditorGUILayout.EndHorizontal(); // 수평 GUI 레이아웃 종료
        }
    }
}
