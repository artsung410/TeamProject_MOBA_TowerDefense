/*using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    MongoClient client = new MongoClient("mongodb+srv://metaverse:metaverse@cluster0.feoedbv.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    void Start()
    {
        
        // 데이터 베이스 명
        database = client.GetDatabase("TowerDefense");
        // 콜렉션 명
        collection = database.GetCollection<BsonDocument>("User_Info");

        //SaveScoreToDateBase(22);

        // 테스트 하고 지워줘야합니다.
        GetScoresFromDataBase();
    }

    public async void SaveScoreToDateBase(int value)
    {
        var document = new BsonDocument { { "user_id", value } };
        await collection.InsertOneAsync(document);
    }

    public async Task<List<HighScore>> GetScoresFromDataBase()
    {
        var allScoreTask = collection.FindAsync(new BsonDocument());
        Debug.Log($"allScoreTask : {allScoreTask}");
        var scoreAwaited = await allScoreTask;
        Debug.Log($"scoreAwaited : {scoreAwaited}");
        List<HighScore> highScores = new List<HighScore>();
        foreach (var score in scoreAwaited.ToString())
        {
            highScores.Add(Deserialize(score.ToString()));
            Debug.Log($"highScores : {highScores[score]}");
        }

        return highScores;
    }

    private HighScore Deserialize(string rawJson)
    {
        var highScore = new HighScore();

        // userName이 200인 데이터를 찾는다.
        //var filter = Builders<BsonDocument>.Filter.Eq("user_id", 22);

        // .TiList()는 해당 조건의 전체 데이터를 찾고
        //var result1 = collection.Find(filter).ToList();
        // .FirstOrDefault()는 해당 조건의 첫번째 하나의 데이터를 찾는다
        //var result2 = collection.Find(filter).FirstOrDefault();

        // userName을 Key로 Value를 찾아서 출력
        //string Roomname = result1[0].GetElement("userName").Value.ToString();
        //Debug.Log(Roomname);

        return highScore;
    }
}

public class HighScore
{
    public string UserName { get; set; }
    public int Score { get; set; }
}*/