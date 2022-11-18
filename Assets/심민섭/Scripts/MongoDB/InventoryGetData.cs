using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ������, Ÿ�� �κ��丮���� �����͸� ������ �����Ѵ�.
public class InventoryGetData : MonoBehaviour
{
    public static InventoryGetData instance;
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // ��Ÿ �κ��丮(ī����)
    // ���� �κ��丮(���� ī��)
    // ������ �κ��丮(������ ī��)
    // �ϻ��� �κ��丮(����)
    // ���� �κ��丮(���� ī��)
    // Ÿ�� �κ��丮(Ÿ�� ī��)

    // ������ �ִ� �������� ������ ����Ʈ
    public List<GameObject> otherInventoryData = new List<GameObject>();
    public List<GameObject> warriorInventoryData = new List<GameObject>();
    public List<GameObject> wizardInventoryData = new List<GameObject>();
    public List<GameObject> inherenceInventoryData = new List<GameObject>();
    public List<GameObject> towerInventoryData = new List<GameObject>();

    // DB���� ���� �����͸� ������ ��
    public Dictionary<string, BsonValue> otherItem = new Dictionary<string, BsonValue>();
    public Dictionary<string, BsonValue> warriorItem = new Dictionary<string, BsonValue>();
    public Dictionary<string, BsonValue> wizardItem = new Dictionary<string, BsonValue>();
    public Dictionary<string, BsonValue> inherenceItem = new Dictionary<string, BsonValue>();
    public Dictionary<string, BsonValue> towerItem = new Dictionary<string, BsonValue>();

    public int haveCardCnt;
    public int warriorCardCnt;
    public int wizardCardCnt;
    public int inherenceCardCnt;
    public int towerCardCnt;
    public int otherItemCnt;

    // Other �κ��丮
    private GameObject otherInventory;
    // Warrior �κ��丮
    private GameObject warriorInventory;
    // Wizard �κ��丮
    private GameObject wizardInventory;
    // Tower �κ��丮
    private GameObject towerInventory;
    // Inherence �κ��丮
    private GameObject inherenceInventory;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Other �κ��丮
        otherInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject; // Slots
        // Warrior �κ��丮
        warriorInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Wizard �κ��丮
        wizardInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Inherence �κ��丮
        inherenceInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Tower �κ��丮
        towerInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    // �Լ��� ȣ���ϸ� �κ��丮 ���� �ִ� �������� ����Ʈ�� �����Ѵ�.
    public void GetItemInInventoryData()
    {
        otherInventoryData.Clear();
        warriorInventoryData.Clear();
        wizardInventoryData.Clear();
        inherenceInventoryData.Clear();
        towerInventoryData.Clear();

        for (int i = 0; i < otherInventory.transform.childCount; i++)
        {
            Debug.Log("��Ÿ ������ ���� ����");
            if (otherInventory.transform.GetChild(i).childCount != 0)
            {
                otherInventoryData.Add(otherInventory.transform.GetChild(i).GetChild(0).gameObject);
                otherItemCnt += otherInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("��Ÿ ������ ���� �Ϸ�");
        }
        for (int i = 0; i < warriorInventory.transform.childCount; i++)
        {
            Debug.Log("���� ������ ���� ����");
            if (warriorInventory.transform.GetChild(i).childCount != 0)
            {
                warriorInventoryData.Add(warriorInventory.transform.GetChild(i).GetChild(0).gameObject);
                warriorCardCnt += warriorInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("���� ������ ���� �Ϸ�");
        }
        
        for (int i = 0; i < wizardInventory.transform.childCount; i++)
        {
            Debug.Log("������ ������ ���� ����");
            if (wizardInventory.transform.GetChild(i).childCount != 0)
            {
                wizardInventoryData.Add(wizardInventory.transform.GetChild(i).GetChild(0).gameObject);
                wizardCardCnt += wizardInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("������ ������ ���� �Ϸ�");
        }
        for (int i = 0; i < inherenceInventory.transform.childCount; i++)
        {
            Debug.Log("���� ������ ���� ����");
            if (inherenceInventory.transform.GetChild(i).childCount != 0)
            {
                inherenceInventoryData.Add(inherenceInventory.transform.GetChild(i).GetChild(0).gameObject);
                inherenceCardCnt += inherenceInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("���� ������ ���� �Ϸ�");
        }
        for (int i = 0; i < towerInventory.transform.childCount; i++)
        {
            Debug.Log("Ÿ�� ������ ���� ����");
            if (towerInventory.transform.GetChild(i).childCount != 0)
            {
                towerInventoryData.Add(towerInventory.transform.GetChild(i).GetChild(0).gameObject);
                towerCardCnt += towerInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
            }
            Debug.Log("Ÿ�� ������ ���� �Ϸ�");
        }
    }

    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            PutItemInInventoryData();
        }
    }*/
    
    // ��ũ���ͺ� ������Ʈ
    [SerializeField]
    ItemDataBaseList warriorSkillDatabase;
    [SerializeField]
    ItemDataBaseList wizardSkillDatabase;
    [SerializeField]
    ItemDataBaseList inherenceSkillDatabase;
    [SerializeField]
    ItemDataBaseList towerSkillDatabase;

    List<Item> sendItems = new List<Item>();
    List<BsonValue> sendItemValue = new List<BsonValue>();
    // DB���� ���� ������ ������� �������� �����ϰ� �κ��丮�� �־��ش�.
    public void PutItemInInventoryData()
    {
        //warriorItem.Add("ChainAttack1", 1);
        //warriorItem.Add("Whirlwind1", 1);
        //towerItem.Add("Tower_Attack_Flame1", 1);

        // ���� ������
        for (int i = 0; i < warriorItem.Count; i++)
        {
            for (int j = 0; j < warriorSkillDatabase.itemList.Count; j++)
            {
                if (warriorItem.ContainsKey(warriorSkillDatabase.itemList[j].itemName))
                {
                    //Debug.Log(warriorSkillDatabase.itemList[j].itemName);

                    // ������ �ش� �������� �����Ѵ�.
                    sendItems.Add(warriorSkillDatabase.itemList[j]); // <Item>
                    BsonValue value = warriorItem[warriorSkillDatabase.itemList[j].itemName];
                    sendItemValue.Add(value);
                    // ������ ������ �������ش�.
                    warriorItem.Remove(warriorSkillDatabase.itemList[j].itemName);
                    if (warriorItem.Count == 0)
                    {
                        break;
                    }
                    continue;
                }
            }
            if (warriorItem.Count == 0)
            {
                CreateItem(warriorSkillDatabase, warriorInventory);
                break;
            }
            sendItems.Clear();
            sendItemValue.Clear();
        }
        // ������ ������
        for (int i = 0; i < wizardItem.Count; i++)
        {
            for (int j = 0; j < wizardSkillDatabase.itemList.Count; j++)
            {
                if (wizardItem.ContainsKey(wizardSkillDatabase.itemList[j].itemName))
                {
                    //Debug.Log(warriorSkillDatabase.itemList[j].itemName);

                    // ������ �ش� �������� �����Ѵ�.
                    sendItems.Add(wizardSkillDatabase.itemList[j]); // <Item>
                    BsonValue value = wizardItem.TryGetValue(wizardSkillDatabase.itemList[j].itemName, out value);
                    sendItemValue.Add(value);
                    // ������ ������ �������ش�.
                    wizardItem.Remove(wizardSkillDatabase.itemList[j].itemName);
                    if (wizardItem.Count == 0)
                    {
                        break;
                    }
                    continue;
                }
            }
            if (wizardItem.Count == 0)
            {
                CreateItem(wizardSkillDatabase, wizardInventory);
                break;
            }
            sendItems.Clear();
            sendItemValue.Clear();
        }
        // ���� ������
        for (int i = 0; i < inherenceItem.Count; i++)
        {
            for (int j = 0; j < inherenceSkillDatabase.itemList.Count; j++)
            {
                if (inherenceItem.ContainsKey(inherenceSkillDatabase.itemList[j].itemName))
                {
                    //Debug.Log(warriorSkillDatabase.itemList[j].itemName);

                    // ������ �ش� �������� �����Ѵ�.
                    sendItems.Add(inherenceSkillDatabase.itemList[j]); // <Item>
                    BsonValue value = inherenceItem.TryGetValue(inherenceSkillDatabase.itemList[j].itemName, out value);
                    sendItemValue.Add(value);
                    // ������ ������ �������ش�.
                    inherenceItem.Remove(inherenceSkillDatabase.itemList[j].itemName);
                    if (inherenceItem.Count == 0)
                    {
                        break;
                    }
                    continue;
                }
            }
            if (inherenceItem.Count == 0)
            {
                CreateItem(inherenceSkillDatabase, inherenceInventory);
                break;
            }
            sendItems.Clear();
            sendItemValue.Clear();
        }
        // Ÿ�� ������
        for (int i = 0; i < towerItem.Count; i++)
        {
            for (int j = 0; j < towerSkillDatabase.itemList.Count; j++)
            {
                if (towerItem.ContainsKey(towerSkillDatabase.itemList[j].itemName))
                {
                    //Debug.Log(warriorSkillDatabase.itemList[j].itemName);

                    // ������ �ش� �������� �����Ѵ�.
                    sendItems.Add(towerSkillDatabase.itemList[j]); // <Item>
                    BsonValue value = towerItem.TryGetValue(towerSkillDatabase.itemList[j].itemName, out value);
                    sendItemValue.Add(value);
                    // ������ ������ �������ش�.
                    towerItem.Remove(towerSkillDatabase.itemList[j].itemName);
                    if (towerItem.Count == 0)
                    {
                        break;
                    }
                    continue;
                }
            }
            if (towerItem.Count == 0)
            {
                CreateItem(towerSkillDatabase, towerInventory);
                break;
            }
            sendItems.Clear();
            sendItemValue.Clear();
        }
        otherItem.Clear();
        warriorItem.Clear();
        wizardItem.Clear();
        inherenceItem.Clear();
        towerItem.Clear();
    }

    [SerializeField]
    private GameObject prefabItem;
    // ������ ����
    private void CreateItem(ItemDataBaseList itemDatabase, GameObject inventory)
    {
        if (sendItems.Count != 0)
        {
            for (int i = 0; i < sendItems.Count; i++)
            {
                // ������ ������ �־��ֱ�
                ItemStruct itemStruct = new ItemStruct();
                itemStruct.itemName = sendItems[i].itemName;
                itemStruct.ClassType = sendItems[i].ClassType;
                itemStruct.itemID = sendItems[i].itemID;
                itemStruct.itemValue = (int)sendItemValue[i];
                itemStruct.itemDesc = sendItems[i].itemDesc;
                itemStruct.itemIcon = sendItems[i].itemIcon;
                itemStruct.itemModel = sendItems[i].itemModel;
                itemStruct.inGameData = sendItems[i].inGameData;
                itemStruct.itemType = sendItems[i].itemType;
                itemStruct.itemWeight = sendItems[i].itemWeight;
                itemStruct.maxStack = sendItems[i].maxStack;
                itemStruct.indexItemInList = sendItems[i].indexItemInList;
                itemStruct.rarity = sendItems[i].rarity;
                itemStruct.itemAttributes = sendItems[i].itemAttributes;
                itemStruct.specialPrefabs = sendItems[i].specialPrefabs;
                itemStruct.buffDatas = sendItems[i].buffDatas;

                // ������ Ʋ ����� ������ �־��ֱ�
                GameObject itemObjProduce = (GameObject)Instantiate(prefabItem);
                ItemOnObject itemProduce = itemObjProduce.GetComponent<ItemOnObject>();
                itemProduce.item.itemName = itemStruct.itemName;
                itemProduce.item.ClassType = itemStruct.ClassType;
                itemProduce.item.itemID = itemStruct.itemID;
                itemProduce.item.itemDesc = itemStruct.itemDesc;
                itemProduce.item.itemIcon = itemStruct.itemIcon;
                itemProduce.item.itemModel = itemStruct.itemModel;
                itemProduce.item.inGameData = itemStruct.inGameData;
                itemProduce.item.itemValue = itemStruct.itemValue;
                itemProduce.item.itemType = itemStruct.itemType;
                itemProduce.item.itemWeight = itemStruct.itemWeight;
                itemProduce.item.maxStack = itemStruct.maxStack;
                itemProduce.item.indexItemInList = itemStruct.indexItemInList;
                itemProduce.item.rarity = itemStruct.rarity;
                itemProduce.item.itemAttributes = itemStruct.itemAttributes;
                itemProduce.item.specialPrefabs = itemStruct.specialPrefabs;
                itemProduce.item.buffDatas = itemStruct.buffDatas;

                // ������ ��������� �κ��丮�� �Ű� ��߰���?
                itemObjProduce.transform.SetParent(inventory.transform.GetChild(i));
                itemObjProduce.transform.localPosition = Vector3.zero;
                itemObjProduce.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
            sendItems.Clear();
        }
    }    
}
