using UnityEngine;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

// 뽑기, 강화 등 아이템의 변동을 있을때, 실행되고 변경된 아이템만 업데이트가 이뤄진다.
// 뽑기 시, 강화 시, 카드 팩 구매 시
public class DataBaseUpdater : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    public static DataBaseUpdater instance;
    MongoClient server = new MongoClient("mongodb+srv://metaverse:metaverse@cluster0.feoedbv.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;
    private PlayerStorage playerStorage;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        database = server.GetDatabase("TowerDefense");
    }


    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            DrawAfterUpdate();
        }
    }*/

    // 현재 인벤토리에 있는 아이템을 저장하고 비교해서 수량을 올린다.
    public void DrawAfterUpdate()
    {
        // 인벤토리 아이템 저장
        InventoryGetData.instance.GetItemInInventoryData();

        // 전사 아이템
        if (InventoryGetData.instance.warriorInventoryData.Count != 0)
        {
            collection = database.GetCollection<BsonDocument>("User_Warrior_Card_Info");
            playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
            var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
            var document = collection.Find(builder).FirstOrDefault();
            for (int i = 0; i < InventoryGetData.instance.warriorInventoryData.Count; i++)
            {
                for (int j = 2; j < document.Count(); j++)
                {
                    if (document.GetElement(j).Name == InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemName)
                    {
                        var value = document.GetElement(j).Value;
                        // 아이템이 같으면 개수를 업데이트 한다.
                        var filter = Builders<BsonDocument>.Filter.Eq(document.GetElement(j).Name, value);
                        var addr = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
                        var update = Builders<BsonDocument>.Update.Set(document.GetElement(j).Name, addr);
                        collection.UpdateOne(filter, update);
                    }
                }
            }
        }
        // 마법사 아이템
        if (InventoryGetData.instance.wizardInventoryData.Count != 0)
        {
            collection = database.GetCollection<BsonDocument>("User_Wizard_Card_Info");
            playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
            var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
            var document = collection.Find(builder).FirstOrDefault();
            for (int i = 0; i < InventoryGetData.instance.wizardInventoryData.Count; i++)
            {
                for (int j = 2; j < document.Count(); j++)
                {
                    if (document.GetElement(j).Name == InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemName)
                    {
                         var value = document.GetElement(j).Value;
                        // 아이템이 같으면 개수를 업데이트 한다.
                        var filter = Builders<BsonDocument>.Filter.Eq(document.GetElement(j).Name, value);
                        var addr = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
                        var update = Builders<BsonDocument>.Update.Set(document.GetElement(j).Name, addr);
                        collection.UpdateOne(filter, update);
                    }
                }
            }
        }
        // 공용 아이템
        if (InventoryGetData.instance.inherenceInventoryData.Count != 0)
        {
            collection = database.GetCollection<BsonDocument>("User_Inherence_Card_Info");
            playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
            var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
            var document = collection.Find(builder).FirstOrDefault();
            for (int i = 0; i < InventoryGetData.instance.inherenceInventoryData.Count; i++)
            {
                for (int j = 2; j < document.Count(); j++)
                {
                    if (document.GetElement(j).Name == InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemName)
                    {
                        var value = document.GetElement(j).Value;
                        // 아이템이 같으면 개수를 업데이트 한다.
                        var filter = Builders<BsonDocument>.Filter.Eq(document.GetElement(j).Name, value);
                        var addr = InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
                        var update = Builders<BsonDocument>.Update.Set(document.GetElement(j).Name, addr);
                        collection.UpdateOne(filter, update);
                    }
                }
            }
        }
        // 타워 아이템
        if (InventoryGetData.instance.towerInventoryData.Count != 0)
        {
            collection = database.GetCollection<BsonDocument>("User_Tower_Card_Info");
            playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
            var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
            var document = collection.Find(builder).FirstOrDefault();
            for (int i = 0; i < InventoryGetData.instance.towerInventoryData.Count; i++)
            {
                for (int j = 2; j < document.Count(); j++)
                {
                    if (document.GetElement(j).Name == InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemName)
                    {
                        var value = document.GetElement(j).Value;
                        // 아이템이 같으면 개수를 업데이트 한다.
                        var filter = Builders<BsonDocument>.Filter.Eq(document.GetElement(j).Name, value);
                        var addr = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
                        var update = Builders<BsonDocument>.Update.Set(document.GetElement(j).Name, addr);
                        collection.UpdateOne(filter, update);
                    }
                }
            }
        }
        // 카드팩 아이템
        cardPackAmountUpdate();


        InventoryGetData.instance.otherInventoryData.Clear();
        InventoryGetData.instance.warriorInventoryData.Clear();
        InventoryGetData.instance.wizardInventoryData.Clear();
        InventoryGetData.instance.inherenceInventoryData.Clear();
        InventoryGetData.instance.towerInventoryData.Clear();
    }


    public void cardPackAmountUpdate()
    {
        // 카드팩 아이템
        if (InventoryGetData.instance.otherInventoryData.Count != 0)
        {
            collection = database.GetCollection<BsonDocument>("User_CardPack_Info");
            playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
            var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
            var document = collection.Find(builder).FirstOrDefault();
            for (int i = 0; i < InventoryGetData.instance.otherInventoryData.Count; i++)
            {
                for (int j = 2; j < document.Count(); j++)
                {
                    if (document.GetElement(j).Name == InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemName)
                    {
                        var value = document.GetElement(j).Value;
                        // 아이템이 같으면 개수를 업데이트 한다.
                        var filter = Builders<BsonDocument>.Filter.Eq(document.GetElement(j).Name, value);
                        var addr = InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
                        var update = Builders<BsonDocument>.Update.Set(document.GetElement(j).Name, addr);
                        collection.UpdateOne(filter, update);
                    }
                }
                
            }
        }
    }

    // 카드팩 빼는 함수
    public void cardPackSubUpdate(string subName, int subNumber)
    {
        // 사용 수 만큼 뺀다.
        collection = database.GetCollection<BsonDocument>("User_CardPack_Info");
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        var document = collection.Find(builder).FirstOrDefault();

        var value_index = document.IndexOfName(subName);
        var value_value = document.GetElement(value_index).Value;
        var filter = Builders<BsonDocument>.Filter.Eq(document.GetElement(value_index).Name, value_value);
        var sub = value_value.ToInt32() - subNumber;
        var update = Builders<BsonDocument>.Update.Set(document.GetElement(value_index).Name, sub);
        collection.UpdateOne(filter, update);
    }


}
