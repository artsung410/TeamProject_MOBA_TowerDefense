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
    // 로딩에서 호출

    // 이긴 사람이 나오면 이긴 사람의 id를 가지고 호출
    public IEnumerator WinnerAPICaller_S()
    {
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/dappx/declare-winner";

        // 배팅 ID을 가져오고 이긴사람의 id
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

        // 직렬화
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

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // 데이터 저장
            WinnerManager.instance.winAmount = jsonPlayer["data"]["amount_won"].ToString();
            Debug.Log("WinnerCaller Data Save Complited");
            WinnerManager.instance.winner = true;
        }

        Destroy(gameObject);
    }

}



