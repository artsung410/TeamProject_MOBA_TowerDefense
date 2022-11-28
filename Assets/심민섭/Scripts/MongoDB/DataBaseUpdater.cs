using UnityEngine;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

// �̱�, ��ȭ �� �������� ������ ������, ����ǰ� ����� �����۸� ������Ʈ�� �̷�����.
// �̱� ��, ��ȭ ��, ī�� �� ���� ��
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

    // ���� �κ��丮�� �ִ� �������� �����ϰ� ���ؼ� ������ �ø���.
    public void DrawAfterUpdate()
    {
        // �κ��丮 ������ ����
        InventoryGetData.instance.GetItemInInventoryData();

        // ���� ������
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
                        // �������� ������ ������ ������Ʈ �Ѵ�.
                        var filter = Builders<BsonDocument>.Filter.Eq(document.GetElement(j).Name, value);
                        var addr = InventoryGetData.instance.warriorInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
                        var update = Builders<BsonDocument>.Update.Set(document.GetElement(j).Name, addr);
                        collection.UpdateOne(filter, update);
                    }
                }
            }
        }
        // ������ ������
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
                        // �������� ������ ������ ������Ʈ �Ѵ�.
                        var filter = Builders<BsonDocument>.Filter.Eq(document.GetElement(j).Name, value);
                        var addr = InventoryGetData.instance.wizardInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
                        var update = Builders<BsonDocument>.Update.Set(document.GetElement(j).Name, addr);
                        collection.UpdateOne(filter, update);
                    }
                }
            }
        }
        // ���� ������
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
                        // �������� ������ ������ ������Ʈ �Ѵ�.
                        var filter = Builders<BsonDocument>.Filter.Eq(document.GetElement(j).Name, value);
                        var addr = InventoryGetData.instance.inherenceInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
                        var update = Builders<BsonDocument>.Update.Set(document.GetElement(j).Name, addr);
                        collection.UpdateOne(filter, update);
                    }
                }
            }
        }
        // Ÿ�� ������
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
                        // �������� ������ ������ ������Ʈ �Ѵ�.
                        var filter = Builders<BsonDocument>.Filter.Eq(document.GetElement(j).Name, value);
                        var addr = InventoryGetData.instance.towerInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
                        var update = Builders<BsonDocument>.Update.Set(document.GetElement(j).Name, addr);
                        collection.UpdateOne(filter, update);
                    }
                }
            }
        }
        // ī���� ������
        cardPackAmountUpdate();


        InventoryGetData.instance.otherInventoryData.Clear();
        InventoryGetData.instance.warriorInventoryData.Clear();
        InventoryGetData.instance.wizardInventoryData.Clear();
        InventoryGetData.instance.inherenceInventoryData.Clear();
        InventoryGetData.instance.towerInventoryData.Clear();
    }


    public void cardPackAmountUpdate()
    {
        // ī���� ������
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
                        // �������� ������ ������ ������Ʈ �Ѵ�.
                        var filter = Builders<BsonDocument>.Filter.Eq(document.GetElement(j).Name, value);
                        var addr = InventoryGetData.instance.otherInventoryData[i].GetComponent<ItemOnObject>().item.itemValue;
                        var update = Builders<BsonDocument>.Update.Set(document.GetElement(j).Name, addr);
                        collection.UpdateOne(filter, update);
                    }
                }
                
            }
        }
    }

    // ī���� ���� �Լ�
    public void cardPackSubUpdate(string subName, int subNumber)
    {
        // ��� �� ��ŭ ����.
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
