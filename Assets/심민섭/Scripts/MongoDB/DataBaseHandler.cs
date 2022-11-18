using UnityEngine;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver.Linq;
using System.Linq;


public class DataBaseHandler : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    MongoClient server = new MongoClient("mongodb+srv://metaverse:metaverse@cluster0.feoedbv.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    // 0. 로비로 돌아 왔을 땐, 어떻게 할지 정해야 됨 DB로 불러 올지, 따로 데이터 저장해서 쓸지

    private PlayerStorage playerStorage;
    // 1. 유저 id가 없으면 정보를 인서트해준다.


    private void Start()
    {
        database = server.GetDatabase("TowerDefense");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            USER_INIT_INFO_INSERT();
        }
    }

    [SerializeField]
    private DataBaseInsert init_UserInfo_Insert;

    private void USER_INIT_INFO_INSERT()
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
            Debug.Log("초기 데이터를 삽입합니다.");
            init_UserInfo_Insert.New_DataInsert_User_Info();
            init_UserInfo_Insert.New_DataInsert_User_Card_Info();
            init_UserInfo_Insert.New_DataInsert_User_WarriorCard_Info();
            init_UserInfo_Insert.New_DataInsert_User_WizardCard_Info();
            init_UserInfo_Insert.New_DataInsert_User_InherenceCard_Info();
            init_UserInfo_Insert.New_DataInsert_User_TowerCard_Info();
        }

        if (document != null)
        {
            Debug.Log("아이템을 가져옵니다.");
            // 아이템 데이터 가져오기 시작
            GET_USER_ITEM();
        }   
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
                InventoryGetData.instance.warriorItem.Add(document.GetElement(i).Name, document.GetElement(i).Value);
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
                InventoryGetData.instance.wizardItem.Add(document.GetElement(i).Name, document.GetElement(i).Value);
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
                InventoryGetData.instance.inherenceItem.Add(document.GetElement(i).Name, document.GetElement(i).Value);
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
                InventoryGetData.instance.towerItem.Add(document.GetElement(i).Name, document.GetElement(i).Value);
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }

    private void GET_USER_OTHER_ITEM()
    {
        // 추후 만들자...
    }


    // - DataBaseInsert스크립트에서 함수 호출
    // 2. 유저 id가 있으면 정보를 불러 온다.
    // 3. 뽑기 시 갯수 업데이트
    // 4. 강화시 갯수, 아이템 업데이트
    // 5. 카드 팩 구매했을때 아이템 업데이트

}

