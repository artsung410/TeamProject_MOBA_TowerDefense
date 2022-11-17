using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Collections.Generic;

public class DataBaseUpdater : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    string url = "mongodb+srv://metaverse:metaverse@cluster0.feoedbv.mongodb.net/?retryWrites=true&w=majority";
    MongoClient server;
    //MongoDatabase database;
    //MongoCollection<BsonDocument> collection;
    private PlayerStorage playerStorage;

    private void Start()
    {
        server = new MongoClient(url);
        //database = server.GetServer().GetDatabase("TowerDefense");
    }


    private async void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.U))
        {
            playerStorage = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>();
            //collection = database.GetCollection<BsonDocument>("User_Inherence_Card_Info");
            *//*var filter = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);
            var result2 = collection.Find(filter).FirstOrDefault();
            Debug.Log(result2); *//*
        }*/
    }

    /*public async Task<List<string>> GetScoresFromDataBase()
    {
        var allTask = collection.FindAsync(new BsonDocument());
        var Awaited = await allTask;

        List<string> user_id = new List<string>();
        foreach (var score in Awaited.ToString())
        {
            user_id.Add(Deserialize(score.ToString()));
        }

        return user_id;
    }

    private string Deserialize(string rawJson)
    {
        var highScore = new string();

        // userName이 200인 데이터를 찾는다.
        var filter = Builders<BsonDocument>.Filter.Eq("user_id", playerStorage._id);

        // .TiList()는 해당 조건의 전체 데이터를 찾고
        //var result1 = collection.Find(filter).ToList();
        // .FirstOrDefault()는 해당 조건의 첫번째 하나의 데이터를 찾는다
        var result2 = collection.Find(filter).FirstOrDefault();

        // userName을 Key로 Value를 찾아서 출력
        //string Roomname = result1[0].GetElement("userName").Value.ToString();
        //Debug.Log(Roomname);

        return highScore;
    }*/



}
