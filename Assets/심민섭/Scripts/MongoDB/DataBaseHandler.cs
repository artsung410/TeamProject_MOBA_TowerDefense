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

    // 0. 로비로 돌아 왔을 땐, 어떻게 할지 정해야 됨 DB로 불러 올지, 따로 데이터 저장해서 쓸지

    private PlayerStorage playerStorage;
    // 1. 유저 id가 없으면 정보를 인서트해준다.

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
        // - 유저 ID가 있는지 확인한다.
        playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
        collection = database.GetCollection<BsonDocument>("User_Info");
        var builder = Builders<BsonDocument>.Filter;
        var filter = builder.Eq("user_id", 1111);
        var document = collection.Find(filter);
        Debug.Log(document);
    }
    // - DataBaseInsert스크립트에서 함수 호출
    // 2. 유저 id가 있으면 정보를 불러 온다.
    // 3. 뽑기 시 갯수 업데이트
    // 4. 강화시 갯수, 아이템 업데이트
    // 5. 카드 팩 구매했을때 아이템 업데이트

}

