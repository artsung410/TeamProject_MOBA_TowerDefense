using MongoDB.Bson;
//using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using LitJson;
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

    MongoClient server = new MongoClient("mongodb+srv://metaverse:metaverse@cluster0.feoedbv.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> user_info_collection;

    public struct MyStruct
    {
        public string aa;
        public int bb;
    }

    void Start()
    {
        Debug.Log("������ ���� ����");
        database = server.GetDatabase("TowerDefense");
        user_info_collection = database.GetCollection<BsonDocument>("User_Info");

        // �׽�Ʈ �ڵ�...�ϰ� ��������մϴ�.
        //GetScoresFromDataBase();
        //SaveScoreToDateBase("�޷��̴�!", 777);
        TestDataSave();
    }


    public async void SaveUserInfoToDateBase(string fieldName, int value)
    {
        var document = new BsonDocument { { fieldName, value } };
        await user_info_collection.InsertOneAsync(document);
    }

    /*public async Task<List<HighScore>> GetScoresFromDataBase()
    {
        var allScoreTask = user_info_collection.FindAsync(new BsonDocument());
        var scoreAwaited = await allScoreTask;

        List<HighScore> highScores = new List<HighScore>();
        foreach (var score in scoreAwaited.ToString())
        {
            highScores.Add(Deserialize(score.ToString()));
        }

        return highScores;
    }*/

    /*private HighScore Deserialize(string rawJson)
    {
        var highScore = new HighScore();

        // userName�� 200�� �����͸� ã�´�.
        var filter = Builders<BsonDocument>.Filter.Eq("userName", 200);

        // .TiList()�� �ش� ������ ��ü �����͸� ã��
        //var result1 = collection.Find(filter).ToList();
        // .FirstOrDefault()�� �ش� ������ ù��° �ϳ��� �����͸� ã�´�
        var result2 = user_info_collection.Find(filter).FirstOrDefault();

        // userName�� Key�� Value�� ã�Ƽ� ���
        //string Roomname = result1[0].GetElement("userName").Value.ToString();
        //Debug.Log(Roomname);

        return highScore;
    }*/
    
    private async void TestDataSave()
    {
        Root root = new Root();
        UserInfo userInfo = new UserInfo();
        SkillCard skillCard = new SkillCard();

        

        // skill_card
        skillCard.warrior = 1;
        skillCard.wizard = 2;
        skillCard.Inherence = 3;
        TowerCard towerCard = new TowerCard();
        towerCard.attack = 4;
        towerCard.minion = 5;
        towerCard.buff = 6;
        // user_info
        userInfo.user_id = "1234567890";
        userInfo.nick_name = "abcd";
        userInfo.skill_card = skillCard;
        userInfo.tower_card = towerCard;
        root.user_info = userInfo;

        // ����ȭ
        /*var serializeObject = JsonConvert.SerializeObject(root);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(serializeObject);
        string jsonResult = System.Text.Encoding.UTF8.GetString(jsonToSend);
        var document = new BsonDocument { { jsonResult, jsonResult } };*/
        //await user_info_collection.InsertOneAsync(skillCard);
        // ������ �Ľ�
        //JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

    }
}

public class Root
{
    public UserInfo user_info { get; set; }
}

public class SkillCard
{
    public int warrior { get; set; }
    public int wizard { get; set; }
    public int Inherence { get; set; }
}

public class TowerCard
{
    public int attack { get; set; }
    public int minion { get; set; }
    public int buff { get; set; }
}

public class UserInfo
{
    public string user_id { get; set; }
    public string nick_name { get; set; }
    public SkillCard skill_card { get; set; }
    public TowerCard tower_card { get; set; }
}