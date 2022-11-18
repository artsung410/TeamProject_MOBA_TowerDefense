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

    // 0. �κ�� ���� ���� ��, ��� ���� ���ؾ� �� DB�� �ҷ� ����, ���� ������ �����ؼ� ����

    private PlayerStorage playerStorage;
    // 1. ���� id�� ������ ������ �μ�Ʈ���ش�.


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
        // �÷��̾� ���ø��� _id ��������
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        // �÷��̾� _id�� DB�� �ִ��� Ȯ���ϱ�
        var builder = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
        // ã�� ������ Null ����
        var document = collection.Find(builder).FirstOrDefault();
        if (document == null)
        {
            Debug.Log("�ʱ� �����͸� �����մϴ�.");
            init_UserInfo_Insert.New_DataInsert_User_Info();
            init_UserInfo_Insert.New_DataInsert_User_Card_Info();
            init_UserInfo_Insert.New_DataInsert_User_WarriorCard_Info();
            init_UserInfo_Insert.New_DataInsert_User_WizardCard_Info();
            init_UserInfo_Insert.New_DataInsert_User_InherenceCard_Info();
            init_UserInfo_Insert.New_DataInsert_User_TowerCard_Info();
        }

        if (document != null)
        {
            Debug.Log("�������� �����ɴϴ�.");
            // ������ ������ �������� ����
            GET_USER_ITEM();
        }   
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
                InventoryGetData.instance.warriorItem.Add(document.GetElement(i).Name, document.GetElement(i).Value);
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
                InventoryGetData.instance.wizardItem.Add(document.GetElement(i).Name, document.GetElement(i).Value);
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
                InventoryGetData.instance.inherenceItem.Add(document.GetElement(i).Name, document.GetElement(i).Value);
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
                InventoryGetData.instance.towerItem.Add(document.GetElement(i).Name, document.GetElement(i).Value);
            }
        }
        InventoryGetData.instance.PutItemInInventoryData();
    }

    private void GET_USER_OTHER_ITEM()
    {
        // ���� ������...
    }


    // - DataBaseInsert��ũ��Ʈ���� �Լ� ȣ��
    // 2. ���� id�� ������ ������ �ҷ� �´�.
    // 3. �̱� �� ���� ������Ʈ
    // 4. ��ȭ�� ����, ������ ������Ʈ
    // 5. ī�� �� ���������� ������ ������Ʈ

}

