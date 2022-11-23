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
        
        // ������ ���̽� ��
        database = client.GetDatabase("TowerDefense");
        // �ݷ��� ��
        collection = database.GetCollection<BsonDocument>("User_Info");

        //SaveScoreToDateBase(22);

        // �׽�Ʈ �ϰ� ��������մϴ�.
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

        // userName�� 200�� �����͸� ã�´�.
        //var filter = Builders<BsonDocument>.Filter.Eq("user_id", 22);

        // .TiList()�� �ش� ������ ��ü �����͸� ã��
        //var result1 = collection.Find(filter).ToList();
        // .FirstOrDefault()�� �ش� ������ ù��° �ϳ��� �����͸� ã�´�
        //var result2 = collection.Find(filter).FirstOrDefault();

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
}*/