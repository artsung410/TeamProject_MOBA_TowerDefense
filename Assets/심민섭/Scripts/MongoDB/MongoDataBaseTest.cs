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

    MongoClient client = new MongoClient("���⿡ ����DB string �־��ּ���. ����!");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    void Start()
    {
        //������ ���̽� �� �־��ְ��.
        database = client.GetDatabase("TestDataBase");
        //�ݷ��� �� �־� �ְ��.
        collection = database.GetCollection<BsonDocument>("TestCollection");

        // �׽�Ʈ �ϰ� ��������մϴ�.
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

        // userName�� 200�� �����͸� ã�´�.
        var filter = Builders<BsonDocument>.Filter.Eq("userName", 200);

        // .TiList()�� �ش� ������ ��ü �����͸� ã��
        //var result1 = collection.Find(filter).ToList();
        // .FirstOrDefault()�� �ش� ������ ù��° �ϳ��� �����͸� ã�´�
        var result2 = collection.Find(filter).FirstOrDefault();

        // userName�� Key�� Value�� ã�Ƽ� ���
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