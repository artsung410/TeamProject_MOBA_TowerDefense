using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using UnityEngine.UI;
using Photon.Pun;

public class WinnerAPICaller : MonoBehaviourPunCallbacks
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    private GameObject apiStorageObj;
    private APIStorage aPIStorage;

    private void Awake()
    {
        apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
        aPIStorage = apiStorageObj.GetComponent<APIStorage>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(WinnerAPICaller_S());
        else
            return;
    }
    // �ε����� ȣ��

    // �̱� ����� ������ �̱� ����� id�� ������ ȣ��
    public IEnumerator WinnerAPICaller_S()
    {
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/dappx/declare-winner";

        // ���� ID�� �������� �̱����� id
        WWWForm form = new WWWForm();

        winner winnerBet = new winner();
        winnerBet.betting_id = aPIStorage.betting_id;
        if (GameManager.Instance.winner == "Blue")
        {
            winnerBet.winner_player_id = aPIStorage._id[0];
        }
        else // Red
        {
            winnerBet.winner_player_id = aPIStorage._id[1];
        }
        //winnerBet.winner_player_id = aPIStorage.winner_id;
        winnerBet.match_details = new MatchDetails();

        // ����ȭ
        var serializeObject = JsonConvert.SerializeObject(winnerBet);

        using (UnityWebRequest www = UnityWebRequest.Post(url, serializeObject))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(serializeObject);
            www.uploadHandler.Dispose();
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);


            www.SetRequestHeader("api-key", aPIStorage.apiKey);
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // ������ ����
            WinnerManager.instance.winAmount = jsonPlayer["data"]["amount_won"].ToString();
            Debug.Log("WinnerCaller Data Save Complited");
            WinnerManager.instance.winner = true;
        }

        Destroy(gameObject);
    }

}



