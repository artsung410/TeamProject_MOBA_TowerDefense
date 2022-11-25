using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SendReport : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 저장할 데이터
    // 내 닉네임, 전판 상대방 닉네임, 텍스트

    MongoClient server = new MongoClient("mongodb+srv://metaverse:metaverse@cluster0.feoedbv.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    private void Awake()
    {
        database = server.GetDatabase("TowerDefense");
    }
    // My NickName
    private string GetNickName()
    {
        string myNickName = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>().userName;

        return myNickName;
    }

    [SerializeField]
    private TextMeshProUGUI explanation;

    public async void ReportButton()
    {
        collection = database.GetCollection<BsonDocument>("Report");
        string myNickName = GetNickName();
        string explanation_text = explanation.text;
        var SendReport = new BsonDocument()
        {
            {"MyNickName", myNickName},
            {"YourNickName", "상대방 닉네임"},
            {"explanation", explanation_text},
        };

        await collection.InsertOneAsync(SendReport);
    }
}
