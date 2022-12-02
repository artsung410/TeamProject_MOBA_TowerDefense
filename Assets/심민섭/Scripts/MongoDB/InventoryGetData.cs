using MongoDB.Bson;
using System.Collections;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Dictionary<string, int> otherItem = new Dictionary<string, int>();
    public Dictionary<string, int> warriorItem = new Dictionary<string, int>();
    public Dictionary<string, int> wizardItem = new Dictionary<string, int>();
    public Dictionary<string, int> inherenceItem = new Dictionary<string, int>();
    public Dictionary<string, int> towerItem = new Dictionary<string, int>();

    public int haveCardCnt;
    public int warriorCardCnt;
    public int wizardCardCnt;
    public int inherenceCardCnt;
    public int towerCardCnt;
    public int otherItemCnt;

    // Sprite
    [SerializeField]
    private Sprite warrior_N;
    [SerializeField]
    private Sprite wizard_N;
    [SerializeField]
    private Sprite inherence_N;
    [SerializeField]
    private Sprite attack_N;
    [SerializeField]
    private Sprite minion_N;
    [SerializeField]
    private Sprite buff_N;
    [SerializeField]
    private Sprite random_N;

    [SerializeField]
    private Sprite warrior_P;
    [SerializeField]
    private Sprite wizard_P;
    [SerializeField]
    private Sprite inherence_P;
    [SerializeField]
    private Sprite attack_P;
    [SerializeField]
    private Sprite minion_P;
    [SerializeField]
    private Sprite buff_P;
    [SerializeField]
    private Sprite random_P;
    MongoClient server = new MongoClient("mongodb+srv://metaverse:metaverse@cluster0.feoedbv.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;
    private PlayerStorage playerStorage;

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
        database = server.GetDatabase("TowerDefense");
        // Other �κ��丮
        otherInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
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
        haveCardCnt = 0;
        warriorCardCnt = 0;
        wizardCardCnt = 0;
        inherenceCardCnt = 0;
        towerCardCnt = 0;
        otherItemCnt = 0;
    int stack = 0;
        for (int i = 0; i < otherInventory.transform.childCount; i++)
        {
            Debug.Log("��Ÿ ������ ���� ����");
            if (otherInventory.transform.GetChild(i).childCount != 0)
            {
                int count = otherInventory.transform.GetChild(i).childCount;
                otherInventoryData.Add(otherInventory.transform.GetChild(i).GetChild(0).gameObject);
                otherItemCnt += otherInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
                count--;
                stack++;
                if (count == 0)
                {
                    continue;
                }
            }
            Debug.Log("��Ÿ ������ ���� �Ϸ�");
        }
        if (stack > 0)
            DataBaseHandler.instance.USER_ITEM_TOTAL_CNT_UPDATE("Other", otherItemCnt);

        collection = database.GetCollection<BsonDocument>("User_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder_other = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document_other = collection.Find(builder_other).FirstOrDefault();
        var value_other = document_other.GetElement(7).Value;
        if (stack == 0 && value_other >= 1)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("other", value_other);
            var update = Builders<BsonDocument>.Update.Set("other", 0);
            collection.UpdateOne(filter, update);
        }

        stack = 0;
        for (int i = 0; i < warriorInventory.transform.childCount; i++)
        {
            Debug.Log("���� ������ ���� ����");
            if (warriorInventory.transform.GetChild(i).childCount != 0)
            {
                warriorInventoryData.Add(warriorInventory.transform.GetChild(i).GetChild(0).gameObject);
                warriorCardCnt += warriorInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
                stack++;
            }
            Debug.Log("���� ������ ���� �Ϸ�");
        }
        if (stack > 0)
            DataBaseHandler.instance.USER_ITEM_TOTAL_CNT_UPDATE("Warrior", warriorCardCnt);

        collection = database.GetCollection<BsonDocument>("User_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder_warrior = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document_warrior = collection.Find(builder_warrior).FirstOrDefault();
        var value_warrior = document_warrior.GetElement(3).Value;
        if (stack == 0 && value_warrior >= 1)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("warrior", value_warrior);
            var update = Builders<BsonDocument>.Update.Set("warrior", 0);
            collection.UpdateOne(filter, update);
        }

        stack = 0;
        for (int i = 0; i < wizardInventory.transform.childCount; i++)
        {
            Debug.Log("������ ������ ���� ����");
            if (wizardInventory.transform.GetChild(i).childCount != 0)
            {
                wizardInventoryData.Add(wizardInventory.transform.GetChild(i).GetChild(0).gameObject);
                wizardCardCnt += wizardInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
                stack++;
            }
            Debug.Log("������ ������ ���� �Ϸ�");
        }
        if (stack > 0)
            DataBaseHandler.instance.USER_ITEM_TOTAL_CNT_UPDATE("Wizard", wizardCardCnt);

        collection = database.GetCollection<BsonDocument>("User_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder_wizard = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document_wizard = collection.Find(builder_wizard).FirstOrDefault();
        var value_wizard = document_wizard.GetElement(4).Value;
        if (stack == 0 && value_wizard >= 1)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("wizard", value_wizard);
            var update = Builders<BsonDocument>.Update.Set("wizard", 0);
            collection.UpdateOne(filter, update);
        }


        stack = 0;
        for (int i = 0; i < inherenceInventory.transform.childCount; i++)
        {
            Debug.Log("���� ������ ���� ����");
            if (inherenceInventory.transform.GetChild(i).childCount != 0)
            {
                inherenceInventoryData.Add(inherenceInventory.transform.GetChild(i).GetChild(0).gameObject);
                inherenceCardCnt += inherenceInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
                stack++;
            }
            Debug.Log("���� ������ ���� �Ϸ�");
        }
        if (stack > 0)
            DataBaseHandler.instance.USER_ITEM_TOTAL_CNT_UPDATE("Inherence", inherenceCardCnt);

        collection = database.GetCollection<BsonDocument>("User_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder_inherence = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document_inherence = collection.Find(builder_inherence).FirstOrDefault();
        var value_inherence = document_inherence.GetElement(5).Value;
        if (stack == 0 && value_inherence >= 1)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("inherence", value_inherence);
            var update = Builders<BsonDocument>.Update.Set("inherence", 0);
            collection.UpdateOne(filter, update);
        }


        stack = 0;
        for (int i = 0; i < towerInventory.transform.childCount; i++)
        {
            Debug.Log("Ÿ�� ������ ���� ����");
            if (towerInventory.transform.GetChild(i).childCount != 0)
            {
                towerInventoryData.Add(towerInventory.transform.GetChild(i).GetChild(0).gameObject);
                towerCardCnt += towerInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
                stack++;
            }
            Debug.Log("Ÿ�� ������ ���� �Ϸ�");
        }
        if (stack > 0)
            DataBaseHandler.instance.USER_ITEM_TOTAL_CNT_UPDATE("Tower", towerCardCnt);

        collection = database.GetCollection<BsonDocument>("User_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder_tower = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document_tower = collection.Find(builder_tower).FirstOrDefault();
        var value_tower = document_tower.GetElement(5).Value;
        if (stack == 0 && value_tower >= 1)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("tower", value_tower);
            var update = Builders<BsonDocument>.Update.Set("tower", 0);
            collection.UpdateOne(filter, update);
        }


        // ��� ī�带 ���ؼ� total�� ������Ʈ�� �Ѵ�.
        int total = otherItemCnt + warriorCardCnt + wizardCardCnt + inherenceCardCnt + towerCardCnt;
        DataBaseHandler.instance.USER_ITEM_TOTAL_CNT_UPDATE("Total", total);

        DataBaseUpdater.instance.cardPackAmountUpdate();
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

    // ��Ÿ �������� ���� ����Ʈ
    List<string> cardPack = new List<string>();


    List<Item> sendItems = new List<Item>();
    List<int> sendItemValue = new List<int>();
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
                    int value = warriorItem[warriorSkillDatabase.itemList[j].itemName];
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
                    int value = wizardItem[wizardSkillDatabase.itemList[j].itemName];
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
                    int value = inherenceItem[inherenceSkillDatabase.itemList[j].itemName];
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
                    int value = towerItem[towerSkillDatabase.itemList[j].itemName];
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

        cardPack.Clear();
        sendItemValue.Clear();

        // ī����(��Ÿ) ������
        // �Ф�
        foreach (var itemKey in otherItem)
        {
            cardPack.Add(itemKey.Key);
        }
        foreach (var itemVlaue in otherItem)
        {
            sendItemValue.Add(itemVlaue.Value);
        }

        for (int i = 0; i < otherItem.Count; i++)
        {
            GameObject itemObjProduce = (GameObject)Instantiate(prefabItem);
            ItemOnObject itemProduce = itemObjProduce.GetComponent<ItemOnObject>();
            //itemProduce.item.itemName = cardPack[i];
            bool packCheck = cardPack[i].Contains("_P");
            if (packCheck)
            {
                itemProduce.item.ClassType = "Premium";

                if (cardPack[i].Contains("Warrior"))
                {
                    itemProduce.item.itemName = "Warrior Skill_P";
                    itemProduce.item.itemIcon = warrior_P;
                }
                else if (cardPack[i].Contains("Wizard"))
                {
                    itemProduce.item.itemName = "Wizard Skill_P";
                    itemProduce.item.itemIcon = wizard_P;
                }
                else if (cardPack[i].Contains("Inherence"))
                {
                    itemProduce.item.itemName = "Common Skill_P";
                    itemProduce.item.itemIcon = inherence_P;
                }
                else if (cardPack[i].Contains("Attack"))
                {
                    itemProduce.item.itemName = "Attack Tower_P";
                    itemProduce.item.itemIcon = attack_P;
                }
                else if (cardPack[i].Contains("Minion"))
                {
                    itemProduce.item.itemName = "Minion Tower_P";
                    itemProduce.item.itemIcon = minion_P;
                }
                else if (cardPack[i].Contains("Buff"))
                {
                    itemProduce.item.itemName = "Buff & Debuff Tower_P";
                    itemProduce.item.itemIcon = buff_P;

                }
                else if (cardPack[i].Contains("Random"))
                {
                    itemProduce.item.itemName = "Random Tower_P";
                    itemProduce.item.itemIcon = random_P;
                }
            }
            else
            {
                itemProduce.item.ClassType = "Nomal";

                if (cardPack[i].Contains("Warrior"))
                {
                    itemProduce.item.itemName = "Warrior Skill";
                    itemProduce.item.itemIcon = warrior_N;
                }
                else if (cardPack[i].Contains("Wizard"))
                {
                    itemProduce.item.itemName = "Wizard Skill";
                    itemProduce.item.itemIcon = wizard_N;
                }
                else if (cardPack[i].Contains("Common"))
                {
                    itemProduce.item.itemName = "Common Skill";
                    itemProduce.item.itemIcon = inherence_N;
                }
                else if (cardPack[i].Contains("Attack"))
                {
                    itemProduce.item.itemName = "Attack Tower";
                    itemProduce.item.itemIcon = attack_N;
                }
                else if (cardPack[i].Contains("Minion"))
                {
                    itemProduce.item.itemName = "Minion Tower";
                    itemProduce.item.itemIcon = minion_N;
                }
                else if (cardPack[i].Contains("Buff"))
                {
                    itemProduce.item.itemName = "Buff & Debuff Tower";
                    itemProduce.item.itemIcon = buff_N;

                }
                else if (cardPack[i].Contains("Random"))
                {
                    itemProduce.item.itemName = "Random Tower";
                    itemProduce.item.itemIcon = random_N;
                }
            }
            itemProduce.item.itemID = 0;
            itemProduce.item.itemDesc = "Card Pack";
            itemProduce.item.itemValue = sendItemValue[i];
            itemProduce.item.itemType = ItemType.Consumable;
            itemProduce.item.maxStack = 100;
            itemProduce.item.indexItemInList = 99;
            itemProduce.item.rarity = 0;
            
            // ������ ��������� �κ��丮�� �Ű� ��߰���?
            itemObjProduce.transform.SetParent(otherInventory.transform.GetChild(i));
            itemObjProduce.transform.localPosition = Vector3.zero;
            itemObjProduce.transform.GetChild(1).localPosition = new Vector3(35f, -30f, 0f);
            itemObjProduce.transform.GetChild(1).GetComponent<Text>().fontSize = 20;
            itemObjProduce.transform.localScale = new Vector3(1f, 1f, 1f);
            itemObjProduce.transform.GetChild(0).localScale = new Vector3(2f, 2f, 2f);
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
                itemStruct.itemValue = sendItemValue[i];
                itemStruct.itemDesc = sendItems[i].itemDesc;
                itemStruct.itemIcon = sendItems[i].itemIcon;
                itemStruct.objType = sendItems[i].objType;
                itemStruct.itemType = sendItems[i].itemType;
                itemStruct.towerData = sendItems[i].towerData;
                itemStruct.itemWeight = sendItems[i].itemWeight;
                itemStruct.maxStack = sendItems[i].maxStack;
                itemStruct.indexItemInList = sendItems[i].indexItemInList;
                itemStruct.rarity = sendItems[i].rarity;
                itemStruct.skillData = sendItems[i].skillData;

                // ������ Ʋ ����� ������ �־��ֱ�
                GameObject itemObjProduce = (GameObject)Instantiate(prefabItem);
                ItemOnObject itemProduce = itemObjProduce.GetComponent<ItemOnObject>();
                itemProduce.item.itemName = itemStruct.itemName;
                itemProduce.item.ClassType = itemStruct.ClassType;
                itemProduce.item.itemID = itemStruct.itemID;
                itemProduce.item.itemDesc = itemStruct.itemDesc;
                itemProduce.item.itemIcon = itemStruct.itemIcon;
                itemProduce.item.objType = itemStruct.objType;
                itemProduce.item.itemValue = itemStruct.itemValue;
                itemProduce.item.itemType = itemStruct.itemType;
                itemProduce.item.itemWeight = itemStruct.itemWeight;
                itemProduce.item.maxStack = itemStruct.maxStack;
                itemProduce.item.indexItemInList = itemStruct.indexItemInList;
                itemProduce.item.rarity = itemStruct.rarity;
                itemProduce.item.towerData = itemStruct.towerData;
                itemProduce.item.skillData = itemStruct.skillData;

                // ������ ��������� �κ��丮�� �Ű� ��߰���?
                itemObjProduce.transform.SetParent(inventory.transform.GetChild(i));
                itemObjProduce.transform.localPosition = Vector3.zero;
                itemObjProduce.transform.localScale = new Vector3(0.55f, 0.7f, 0f);
            }
            sendItems.Clear();
        }
    }    
}
