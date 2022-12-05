using UnityEngine;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Threading.Tasks;

public class DataBaseHandler : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    MongoClient server = new MongoClient("mongodb+srv://metaverse:metaverse@cluster0.feoedbv.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    public static DataBaseHandler instance;

    // ù �α������� Ȯ��
    public bool firstLogin;

    private void Awake()
    {
        instance = this;
    }

    // 0. �κ�� ���� ���� ��, ��� ���� ���ؾ� �� DB�� �ҷ� ����, ���� ������ �����ؼ� ����

    private PlayerStorage playerStorage;
    // 1. ���� id�� ������ ������ �μ�Ʈ���ش�.


    private void Start()
    {
        database = server.GetDatabase("TowerDefense");
        //USER_INIT_INFO_INSERT();
    }

    // ���� ���� �μ�Ʈ(�ʱ⿡�� ����ؾ��Ѵ�.)
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Test();
        }
    }*/

    /*public class Nomal
    {
        public string warrior { get; set; }
        public string wizard { get; set; }
    }

    public class Premium
    {
        public string warrior { get; set; }
        public string wizard { get; set; }
    }

    public class Root
    {
        public Nomal Nomal { get; set; }
        public Premium Premium { get; set; }
    }

    public async void Test()
    {
        Nomal nomal = new Nomal();
        nomal.warrior = "1111";
        nomal.wizard = "2222";

        Premium premium = new Premium();
        premium.warrior = "3333";
        premium.warrior = "4444";

        Root root = new Root();
        root.Nomal = nomal;
        root.Premium = premium;
        IMongoCollection<Root> collection;
        collection = database.GetCollection<Root>("TestCollection");
        Debug.Log("���� ����");
        await collection.InsertOneAsync(root);
    }*/




    // User Item Total Item Count
    public void USER_ITEM_TOTAL_CNT_UPDATE(string name, int cnt)
    {
        collection = database.GetCollection<BsonDocument>("User_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        // �÷��̾� _id�� DB�� �ִ��� Ȯ���ϱ�
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        // ã�� ������ Null ����
        var document = collection.Find(builder).FirstOrDefault();

        if (name == "Total")
        {
            var feild = "total_cnt";
            var value = document.GetElement(feild).Value;
            var filter = Builders<BsonDocument>.Filter.Eq(feild, value);
            //var addValue = (int)cnt + (int)value;
            var update = Builders<BsonDocument>.Update.Set(feild, cnt);
            collection.UpdateOne(filter, update);
        }
        else if (name == "Warrior")
        {
            var feild = "warrior";
            var value = document.GetElement(feild).Value;
            var filter = Builders<BsonDocument>.Filter.Eq(feild, value);
            //var addValue = (int)cnt + (int)value;
            var update = Builders<BsonDocument>.Update.Set(feild, cnt);
            collection.UpdateOne(filter, update);
        }
        else if (name == "Wizard")
        {
            var feild = "wizard";
            var value = document.GetElement(feild).Value;
            var filter = Builders<BsonDocument>.Filter.Eq(feild, value);
            //var addValue = (int)cnt + (int)value;
            var update = Builders<BsonDocument>.Update.Set(feild, cnt);
            collection.UpdateOne(filter, update);
        }
        else if (name == "Inherence")
        {
            var feild = "inherence";
            var value = document.GetElement(feild).Value;
            var filter = Builders<BsonDocument>.Filter.Eq(feild, value);
            //var addValue = (int)cnt + (int)value;
            var update = Builders<BsonDocument>.Update.Set(feild, cnt);
            collection.UpdateOne(filter, update);
        }
        else if (name == "Tower")
        {
            var feild = "tower";
            var value = document.GetElement(feild).Value;
            var filter = Builders<BsonDocument>.Filter.Eq(feild, value);
            //var addValue = (int)cnt + (int)value;
            var update = Builders<BsonDocument>.Update.Set(feild, cnt);
            collection.UpdateOne(filter, update);
        }
        else if (name == "Other")
        {
            var feild = "other";
            var value = document.GetElement(feild).Value;
            var filter = Builders<BsonDocument>.Filter.Eq(feild, value);
            //var addValue = (int)cnt + (int)value;
            var update = Builders<BsonDocument>.Update.Set(feild, cnt);
            collection.UpdateOne(filter, update);
        }

    }

    [SerializeField]
    private DataBaseInsert init_UserInfo_Insert;

    public async void USER_INIT_INFO_INSERT()
    {
        collection = database.GetCollection<BsonDocument>("User_Info");
        // �÷��̾� ���ø��� _id ��������
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        // �÷��̾� _id�� DB�� �ִ��� Ȯ���ϱ�
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        // ã�� ������ Null ����
        var document = collection.Find(builder).FirstOrDefault();
        if (document == null)
        {
            firstLogin = true;
            Debug.Log("�ʱ� �����͸� �����մϴ�.");
            init_UserInfo_Insert.New_DataInsert_User_Info();
            init_UserInfo_Insert.New_DataInsert_User_Card_Info();
            init_UserInfo_Insert.New_DataInsert_User_WarriorCard_Info();
            init_UserInfo_Insert.New_DataInsert_User_WizardCard_Info();
            init_UserInfo_Insert.New_DataInsert_User_InherenceCard_Info();
            init_UserInfo_Insert.New_DataInsert_User_TowerCard_Info();
            init_UserInfo_Insert.New_DataInsert_Other_CardPack_Info();
            await FirstLoginITtemInput();
        }

        if (document != null)
        {
            firstLogin = false;
            Debug.Log("�������� �����ɴϴ�.");
            // ������ ������ �������� ����
            GET_USER_ITEM();
        }
    }

    // �ʱ� DB������ ���� ��, ������ ����(ù �α��ζ� �� �ѹ��� �����Ѵ�.)
    private async Task FirstLoginITtemInput()
    {
        await Task.Delay(1000);
        GET_USER_WARRIOR_ITEM();
        GET_USER_WIZARD_ITEM();
        GET_USER_IHERENCE_ITEM();
        GET_USER_TOWER_ITEM();
        GET_USER_OTHER_ITEM();
    }


    private void GET_USER_ITEM()
    {
        // 1. DB�� �ִ� �������� �˻��ؼ� �����Ѵ�.
        collection = database.GetCollection<BsonDocument>("User_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();
        if (document == null)
            return;
        for (int i = 0; i < document.Count(); i++)
        {
            if (document.GetElement(i).Name == "total_cnt")
            {
                if (document.GetElement(i).Value == 0)
                {
                    return;
                }
                else
                {
                    break;
                }
            }
        }

        // total_cnt�� 1�̻� �� ���� - �� ������ ������ ������ �ľ��Ѵ�.
        for (int i = 0; i < document.Count(); i++)
        {
            if (document.GetElement(i).Name == "warrior")
            {
                if (document.GetElement(i).Value == 0)
                {
                    continue;
                }
                else
                {
                    GET_USER_WARRIOR_ITEM();
                }
            }
            if (document.GetElement(i).Name == "wizard")
            {
                if (document.GetElement(i).Value == 0)
                {
                    continue;
                }
                else
                {
                    GET_USER_WIZARD_ITEM();
                }
            }
            if (document.GetElement(i).Name == "inherence")
            {
                if (document.GetElement(i).Value == 0)
                {
                    continue;
                }
                else
                {
                    GET_USER_IHERENCE_ITEM();
                }
            }
            if (document.GetElement(i).Name == "tower")
            {
                if (document.GetElement(i).Value == 0)
                {
                    continue;
                }
                else
                {
                    GET_USER_TOWER_ITEM();
                }
            }
            if (document.GetElement(i).Name == "other")
            {
                if (document.GetElement(i).Value == 0)
                {
                    continue;
                }
                else
                {
                    GET_USER_OTHER_ITEM();
                }
            }
        }

        /*Debug.Log(document.Count()); // �ʵ� ����
        Debug.Log(document[0]); // _id
        Debug.Log(document[1]); // id
        Debug.Log(document[2]); // value
        Debug.Log(document[3]); // value
        Debug.Log(document[4]); // value*/

        // - ī�� �� ������ 0�� �ƴ� ������ ã�ƿ���

        // 2. ������ �����͸� InventoryGetData���� �������� �����ϰ� �κ��丮�� �־� �ش�.
    }

    private void GET_USER_WARRIOR_ITEM()
    {
        // ���� ������
        collection = database.GetCollection<BsonDocument>("User_Warrior_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();
        // value�� 0�� �ƴ� �����͸� ã�´�.
        for (int i = 2; i < document.Count(); i++)
        {
            if (document.GetElement(i).Value != 0)
            {
                // �������� �̸��� value���� �ִ´�.
                InventoryGetData.instance.warriorItem.Add(document.GetElement(i).Name, document.GetElement(i).Value.ToInt32());
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }
    private void GET_USER_WIZARD_ITEM()
    {
        // ������ ������
        collection = database.GetCollection<BsonDocument>("User_Wizard_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();
        // value�� 0�� �ƴ� �����͸� ã�´�.
        for (int i = 2; i < document.Count(); i++)
        {
            if (document.GetElement(i).Value != 0)
            {
                // �������� �̸��� value���� �ִ´�.
                InventoryGetData.instance.wizardItem.Add(document.GetElement(i).Name, document.GetElement(i).Value.ToInt32());
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }
    private void GET_USER_IHERENCE_ITEM()
    {
        // ���� ������
        collection = database.GetCollection<BsonDocument>("User_Inherence_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();
        // value�� 0�� �ƴ� �����͸� ã�´�.
        for (int i = 2; i < document.Count(); i++)
        {
            if (document.GetElement(i).Value != 0)
            {
                // �������� �̸��� value���� �ִ´�.
                InventoryGetData.instance.inherenceItem.Add(document.GetElement(i).Name, document.GetElement(i).Value.ToInt32());
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }
    private void GET_USER_TOWER_ITEM()
    {
        // Ÿ�� ������
        collection = database.GetCollection<BsonDocument>("User_Tower_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();
        // value�� 0�� �ƴ� �����͸� ã�´�.
        for (int i = 2; i < document.Count(); i++)
        {
            if (document.GetElement(i).Value != 0)
            {
                // �������� �̸��� value���� �ִ´�.
                InventoryGetData.instance.towerItem.Add(document.GetElement(i).Name, document.GetElement(i).Value.ToInt32());
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }

    private void GET_USER_OTHER_ITEM()
    {
        // ī���� ������
        collection = database.GetCollection<BsonDocument>("User_CardPack_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();
        // value�� 0�� �ƴ� �����͸� ã�´�.
        for (int i = 2; i < document.Count(); i++) // _id(0), user_id(1)
        {
            if (document.GetElement(i).Value != 0) // ������ 0�� �ƴ� ������
            {
                // �������� �̸��� value���� �ִ´�.
                InventoryGetData.instance.otherItem.Add(document.GetElement(i).Name, document.GetElement(i).Value.ToInt32());
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }

}

