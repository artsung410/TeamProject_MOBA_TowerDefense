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

    // ������ ������
    // �� �г���, ���� ���� �г���, �ؽ�Ʈ

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
            {"YourNickName", "�׻�"},
            {"explanation", explanation.ToString()},
        };
        Debug.Log("�Ű� ����Ʈ ����");
        //collection.Insert(SendReport);
    }
}
