using UnityEngine;
using MongoDB.Driver;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            USER_INFO_INSERT();
        }
    }

    private void USER_INFO_INSERT()
    {
        database = server.GetDatabase("TowerDefense");
        // - ���� ID�� �ִ��� Ȯ���Ѵ�.
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        collection = database.GetCollection<BsonDocument>("User_Info");
        var builder = Builders<BsonDocument>.Filter;
        var filter = builder.Eq("user_id", 1111);
        var document = collection.Find(filter);
        Debug.Log(document);
    }
    // - DataBaseInsert��ũ��Ʈ���� �Լ� ȣ��
    // 2. ���� id�� ������ ������ �ҷ� �´�.
    // 3. �̱� �� ���� ������Ʈ
    // 4. ��ȭ�� ����, ������ ������Ʈ
    // 5. ī�� �� ���������� ������ ������Ʈ

}

