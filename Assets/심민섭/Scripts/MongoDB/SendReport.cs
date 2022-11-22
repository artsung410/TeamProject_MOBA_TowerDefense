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

    // ������ ������
    // �� �г���, ���� ���� �г���, �ؽ�Ʈ

    MongoClient server = new MongoClient("mongodb+srv://metaverse:metaverse@cluster0.feoedbv.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

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
        database = server.GetDatabase("TowerDefense");
        collection = database.GetCollection<BsonDocument>("Report");
        string myNickName = GetNickName();

        var SendReport = new BsonDocument()
        {
            {"MyNickName", myNickName},
            {"YourNickName", "���� �г���"},
            {"explanation", explanation.text.ToString()},
        };

        await collection.InsertOneAsync(SendReport);
    }
}
