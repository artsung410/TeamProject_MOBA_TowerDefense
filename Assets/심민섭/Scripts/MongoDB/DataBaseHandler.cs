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

    // 첫 로그인인지 확인
    public bool firstLogin;

    private void Awake()
    {
        instance = this;
    }

    // 0. 로비로 돌아 왔을 땐, 어떻게 할지 정해야 됨 DB로 불러 올지, 따로 데이터 저장해서 쓸지

    private PlayerStorage playerStorage;
    // 1. 유저 id가 없으면 정보를 인서트해준다.


    private void Start()
    {
        database = server.GetDatabase("TowerDefense");
        //USER_INIT_INFO_INSERT();
    }

    // 유저 정보 인서트(초기에만 사용해야한다.)
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
        Debug.Log("구조 생성");
        await collection.InsertOneAsync(root);
    }*/




    // User Item Total Item Count
    public void USER_ITEM_TOTAL_CNT_UPDATE(string name, int cnt)
    {
        collection = database.GetCollection<BsonDocument>("User_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        // 플레이어 _id가 DB에 있는지 확인하기
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        // 찾고 없으면 Null 리턴
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
        // 플레이어 오시리스 _id 가져오기
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        // 플레이어 _id가 DB에 있는지 확인하기
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        // 찾고 없으면 Null 리턴
        var document = collection.Find(builder).FirstOrDefault();
        if (document == null)
        {
            firstLogin = true;
            Debug.Log("초기 데이터를 삽입합니다.");
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
            Debug.Log("아이템을 가져옵니다.");
            // 아이템 데이터 가져오기 시작
            GET_USER_ITEM();
        }
    }

    // 초기 DB데이터 생성 후, 아이템 생성(첫 로그인때 딱 한번만 실행한다.)
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
        // 1. DB에 있는 아이템을 검색해서 저장한다.
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

        // total_cnt가 1이상 시 진행 - 각 직업의 아이템 갯수를 파악한다.
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

        /*Debug.Log(document.Count()); // 필드 개수
        Debug.Log(document[0]); // _id
        Debug.Log(document[1]); // id
        Debug.Log(document[2]); // value
        Debug.Log(document[3]); // value
        Debug.Log(document[4]); // value*/

        // - 카드 총 개수가 0이 아닌 데이터 찾아오기

        // 2. 저장한 데이터를 InventoryGetData에서 아이템을 생성하고 인벤토리에 넣어 준다.
    }

    private void GET_USER_WARRIOR_ITEM()
    {
        // 전사 아이템
        collection = database.GetCollection<BsonDocument>("User_Warrior_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();
        // value가 0이 아닌 데이터를 찾는다.
        for (int i = 2; i < document.Count(); i++)
        {
            if (document.GetElement(i).Value != 0)
            {
                // 아이템의 이름과 value값을 넣는다.
                InventoryGetData.instance.warriorItem.Add(document.GetElement(i).Name, document.GetElement(i).Value.ToInt32());
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }
    private void GET_USER_WIZARD_ITEM()
    {
        // 마법사 아이템
        collection = database.GetCollection<BsonDocument>("User_Wizard_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();
        // value가 0이 아닌 데이터를 찾는다.
        for (int i = 2; i < document.Count(); i++)
        {
            if (document.GetElement(i).Value != 0)
            {
                // 아이템의 이름과 value값을 넣는다.
                InventoryGetData.instance.wizardItem.Add(document.GetElement(i).Name, document.GetElement(i).Value.ToInt32());
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }
    private void GET_USER_IHERENCE_ITEM()
    {
        // 공용 아이템
        collection = database.GetCollection<BsonDocument>("User_Inherence_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();
        // value가 0이 아닌 데이터를 찾는다.
        for (int i = 2; i < document.Count(); i++)
        {
            if (document.GetElement(i).Value != 0)
            {
                // 아이템의 이름과 value값을 넣는다.
                InventoryGetData.instance.inherenceItem.Add(document.GetElement(i).Name, document.GetElement(i).Value.ToInt32());
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }
    private void GET_USER_TOWER_ITEM()
    {
        // 타워 아이템
        collection = database.GetCollection<BsonDocument>("User_Tower_Card_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();
        // value가 0이 아닌 데이터를 찾는다.
        for (int i = 2; i < document.Count(); i++)
        {
            if (document.GetElement(i).Value != 0)
            {
                // 아이템의 이름과 value값을 넣는다.
                InventoryGetData.instance.towerItem.Add(document.GetElement(i).Name, document.GetElement(i).Value.ToInt32());
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }

    private void GET_USER_OTHER_ITEM()
    {
        // 카드팩 아이템
        collection = database.GetCollection<BsonDocument>("User_CardPack_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();
        // value가 0이 아닌 데이터를 찾는다.
        for (int i = 2; i < document.Count(); i++) // _id(0), user_id(1)
        {
            if (document.GetElement(i).Value != 0) // 수량이 0이 아닌 데이터
            {
                // 아이템의 이름과 value값을 넣는다.
                InventoryGetData.instance.otherItem.Add(document.GetElement(i).Name, document.GetElement(i).Value.ToInt32());
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }

}

