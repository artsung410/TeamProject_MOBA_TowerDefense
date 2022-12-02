using MongoDB.Bson;
using System.Collections;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 각 직업별, 타워 인벤토리에서 데이터를 가져와 저장한다.
public class InventoryGetData : MonoBehaviour
{
    public static InventoryGetData instance;
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 기타 인벤토리(카드팩)
    // 전사 인벤토리(전사 카드)
    // 마법사 인벤토리(마법사 카드)
    // 암살자 인벤토리(제외)
    // 공통 인벤토리(공통 카드)
    // 타워 인벤토리(타워 카드)

    // 가지고 있는 아이템을 저장할 리스트
    public List<GameObject> otherInventoryData = new List<GameObject>();
    public List<GameObject> warriorInventoryData = new List<GameObject>();
    public List<GameObject> wizardInventoryData = new List<GameObject>();
    public List<GameObject> inherenceInventoryData = new List<GameObject>();
    public List<GameObject> towerInventoryData = new List<GameObject>();

    // DB에서 받은 데이터를 저장할 곳
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

    // Other 인벤토리
    private GameObject otherInventory;
    // Warrior 인벤토리
    private GameObject warriorInventory;
    // Wizard 인벤토리
    private GameObject wizardInventory;
    // Tower 인벤토리
    private GameObject towerInventory;
    // Inherence 인벤토리
    private GameObject inherenceInventory;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        database = server.GetDatabase("TowerDefense");
        // Other 인벤토리
        otherInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Warrior 인벤토리
        warriorInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Wizard 인벤토리
        wizardInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Inherence 인벤토리
        inherenceInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        // Tower 인벤토리
        towerInventory = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(3).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    // 함수를 호출하면 인벤토리 내에 있는 아이템을 리스트에 저장한다.
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
            Debug.Log("기타 아이템 저장 시작");
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
            Debug.Log("기타 아이템 저장 완료");
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
            Debug.Log("전사 아이템 저장 시작");
            if (warriorInventory.transform.GetChild(i).childCount != 0)
            {
                warriorInventoryData.Add(warriorInventory.transform.GetChild(i).GetChild(0).gameObject);
                warriorCardCnt += warriorInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
                stack++;
            }
            Debug.Log("전사 아이템 저장 완료");
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
            Debug.Log("마법사 아이템 저장 시작");
            if (wizardInventory.transform.GetChild(i).childCount != 0)
            {
                wizardInventoryData.Add(wizardInventory.transform.GetChild(i).GetChild(0).gameObject);
                wizardCardCnt += wizardInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
                stack++;
            }
            Debug.Log("마법사 아이템 저장 완료");
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
            Debug.Log("공통 아이템 저장 시작");
            if (inherenceInventory.transform.GetChild(i).childCount != 0)
            {
                inherenceInventoryData.Add(inherenceInventory.transform.GetChild(i).GetChild(0).gameObject);
                inherenceCardCnt += inherenceInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
                stack++;
            }
            Debug.Log("공통 아이템 저장 완료");
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
            Debug.Log("타워 아이템 저장 시작");
            if (towerInventory.transform.GetChild(i).childCount != 0)
            {
                towerInventoryData.Add(towerInventory.transform.GetChild(i).GetChild(0).gameObject);
                towerCardCnt += towerInventory.transform.GetChild(i).GetChild(0).gameObject.GetComponent<ItemOnObject>().item.itemValue;
                stack++;
            }
            Debug.Log("타워 아이템 저장 완료");
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


        // 모든 카드를 더해서 total에 업데이트를 한다.
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
    
    // 스크립터블 오브젝트
    [SerializeField]
    ItemDataBaseList warriorSkillDatabase;
    [SerializeField]
    ItemDataBaseList wizardSkillDatabase;
    [SerializeField]
    ItemDataBaseList inherenceSkillDatabase;
    [SerializeField]
    ItemDataBaseList towerSkillDatabase;

    // 기타 아이템을 위한 리스트
    List<string> cardPack = new List<string>();


    List<Item> sendItems = new List<Item>();
    List<int> sendItemValue = new List<int>();
    // DB에서 받은 아이템 목록으로 아이템을 생성하고 인벤토리로 넣어준다.
    public void PutItemInInventoryData()
    {
        //warriorItem.Add("ChainAttack1", 1);
        //warriorItem.Add("Whirlwind1", 1);
        //towerItem.Add("Tower_Attack_Flame1", 1);

        // 전사 아이템
        for (int i = 0; i < warriorItem.Count; i++)
        {
            for (int j = 0; j < warriorSkillDatabase.itemList.Count; j++)
            {
                if (warriorItem.ContainsKey(warriorSkillDatabase.itemList[j].itemName))
                {
                    //Debug.Log(warriorSkillDatabase.itemList[j].itemName);

                    // 같으면 해당 아이템을 생성한다.
                    sendItems.Add(warriorSkillDatabase.itemList[j]); // <Item>
                    int value = warriorItem[warriorSkillDatabase.itemList[j].itemName];
                    sendItemValue.Add(value);
                    // 생성을 했으면 삭제해준다.
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
        // 마법사 아이템
        for (int i = 0; i < wizardItem.Count; i++)
        {
            for (int j = 0; j < wizardSkillDatabase.itemList.Count; j++)
            {
                if (wizardItem.ContainsKey(wizardSkillDatabase.itemList[j].itemName))
                {
                    //Debug.Log(warriorSkillDatabase.itemList[j].itemName);

                    // 같으면 해당 아이템을 생성한다.
                    sendItems.Add(wizardSkillDatabase.itemList[j]); // <Item>
                    int value = wizardItem[wizardSkillDatabase.itemList[j].itemName];
                    sendItemValue.Add(value);
                    // 생성을 했으면 삭제해준다.
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
        // 공용 아이템
        for (int i = 0; i < inherenceItem.Count; i++)
        {
            for (int j = 0; j < inherenceSkillDatabase.itemList.Count; j++)
            {
                if (inherenceItem.ContainsKey(inherenceSkillDatabase.itemList[j].itemName))
                {
                    //Debug.Log(warriorSkillDatabase.itemList[j].itemName);

                    // 같으면 해당 아이템을 생성한다.
                    sendItems.Add(inherenceSkillDatabase.itemList[j]); // <Item>
                    int value = inherenceItem[inherenceSkillDatabase.itemList[j].itemName];
                    sendItemValue.Add(value);
                    // 생성을 했으면 삭제해준다.
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
        // 타워 아이템
        for (int i = 0; i < towerItem.Count; i++)
        {
            for (int j = 0; j < towerSkillDatabase.itemList.Count; j++)
            {
                if (towerItem.ContainsKey(towerSkillDatabase.itemList[j].itemName))
                {
                    //Debug.Log(warriorSkillDatabase.itemList[j].itemName);

                    // 같으면 해당 아이템을 생성한다.
                    sendItems.Add(towerSkillDatabase.itemList[j]); // <Item>
                    int value = towerItem[towerSkillDatabase.itemList[j].itemName];
                    sendItemValue.Add(value);
                    // 생성을 했으면 삭제해준다.
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

        // 카드팩(기타) 아이템
        // ㅠㅜ
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
            
            // 아이템 만들었으면 인벤토리로 옮겨 줘야겠지?
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
    // 아이템 생성
    private void CreateItem(ItemDataBaseList itemDatabase, GameObject inventory)
    {
        if (sendItems.Count != 0)
        {
            for (int i = 0; i < sendItems.Count; i++)
            {
                // 아이템 데이터 넣어주기
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

                // 아이템 틀 만들고 데이터 넣어주기
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

                // 아이템 만들었으면 인벤토리로 옮겨 줘야겠지?
                itemObjProduce.transform.SetParent(inventory.transform.GetChild(i));
                itemObjProduce.transform.localPosition = Vector3.zero;
                itemObjProduce.transform.localScale = new Vector3(0.55f, 0.7f, 0f);
            }
            sendItems.Clear();
        }
    }    
}
