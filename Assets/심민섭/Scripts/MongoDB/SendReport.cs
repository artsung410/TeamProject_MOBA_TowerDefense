using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using UnityEngine.UI;

public class SendReport : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 저장할 데이터
    // 내 닉네임, 전판 상대방 닉네임, 텍스트

    MongoClient server = new MongoClient("mongodb+srv://metaverse:metaverse@cluster0.feoedbv.mongodb.net/?retryWrites=true&w=majority");
    //MongoDatabase database;
    //MongoCollection<BsonDocument> collection;

    // My NickName
    private string GetNickName()
    {
        string myNickName = GameObject.FindGameObjectWithTag("GetCaller").GetComponent<PlayerStorage>().userName;

        return myNickName;
    }
    // Your NickName
    // Explanation
    [SerializeField]
    private Text explanation;

    private void Start()
    {
        //database = server.GetServer().GetDatabase("TowerDefense");
    }

    public void ReportButton()
    {
        //collection = database.GetCollection<BsonDocument>("Report");
        string myNickName = GetNickName();

        var SendReport = new BsonDocument()
        {
            {"MyNickName", myNickName},
            {"YourNickName", "그색"},
            {"explanation", explanation.ToString()},
        };
        Debug.Log("신고 리포트 전송");
        //collection.Insert(SendReport);
    }
}
