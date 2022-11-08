using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MongoDataBaseTest : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    MongoClient client = new MongoClient("여기에 몽고DB string 넣어주세요. 찡긋!");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    void Start()
    {
        //데이터 베이스 명 넣어주고요.
        database = client.GetDatabase("TestDataBase");
        //콜렉션 명도 넣어 주고요.
        collection = database.GetCollection<BsonDocument>("TestCollection");

        // 테스트 하고 지워줘야합니다.
        GetScoresFromDataBase();
    }

    public async void SaveScoreToDateBase(string userName, int score)
    {
        var document = new BsonDocument { { userName, score } };
        await collection.InsertOneAsync(document);
    }

    public async Task<List<HighScore>> GetScoresFromDataBase()
    {
        var allScoreTask = collection.FindAsync(new BsonDocument());
        var scoreAwaited = await allScoreTask;

        List<HighScore> highScores = new List<HighScore>();
        foreach (var score in scoreAwaited.ToString())
        {
            highScores.Add(Deserialize(score.ToString()));
        }

        return highScores;
    }

    private HighScore Deserialize(string rawJson)
    {
        var highScore = new HighScore();

        // userName이 200인 데이터를 찾는다.
        var filter = Builders<BsonDocument>.Filter.Eq("userName", 200);

        // .TiList()는 해당 조건의 전체 데이터를 찾고
        //var result1 = collection.Find(filter).ToList();
        // .FirstOrDefault()는 해당 조건의 첫번째 하나의 데이터를 찾는다
        var result2 = collection.Find(filter).FirstOrDefault();

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
    public int Age { get; set; }
}