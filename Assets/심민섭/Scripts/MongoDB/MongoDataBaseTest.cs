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
        Debug.Log("데이터 연결 시작");
        database = server.GetDatabase("TowerDefense");
        user_info_collection = database.GetCollection<BsonDocument>("User_Info");

        // 테스트 코드...하고 지워줘야합니다.
        //GetScoresFromDataBase();
        //SaveScoreToDateBase("메롱이다!", 777);
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

        // userName이 200인 데이터를 찾는다.
        var filter = Builders<BsonDocument>.Filter.Eq("userName", 200);

        // .TiList()는 해당 조건의 전체 데이터를 찾고
        //var result1 = collection.Find(filter).ToList();
        // .FirstOrDefault()는 해당 조건의 첫번째 하나의 데이터를 찾는다
        var result2 = user_info_collection.Find(filter).FirstOrDefault();

        // userName을 Key로 Value를 찾아서 출력
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

        // 직렬화
        /*var serializeObject = JsonConvert.SerializeObject(root);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(serializeObject);
        string jsonResult = System.Text.Encoding.UTF8.GetString(jsonToSend);
        var document = new BsonDocument { { jsonResult, jsonResult } };*/
        //await user_info_collection.InsertOneAsync(skillCard);
        // 데이터 파싱
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