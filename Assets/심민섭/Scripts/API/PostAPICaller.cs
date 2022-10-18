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

public class PostAPICaller : MonoBehaviourPunCallbacks
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################
    private GameObject apiStorageObj;
    private APIStorage aPIStorage;
    private void Start()
    {
        apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
        aPIStorage = apiStorageObj.GetComponent<APIStorage>();

        StartCoroutine(PostPlaveBetCaller());
    }

    // 호출 정보 : message, betting_id
    public IEnumerator PostPlaveBetCaller()
    {
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/zera/place-bet";

        // 여기선 두명의 세션 아이디를 가져와야함.
        WWWForm form = new WWWForm();

        placeBet placeBet = new placeBet();
        placeBet.players_session_id = new string[2];
        placeBet.players_session_id[0] = aPIStorage.session_id[0];
        placeBet.players_session_id[1] = aPIStorage.MetaMaskSessionID;
        placeBet.bet_id = aPIStorage.bet_id[0]; // 일단은 0 값을 쓴다. 플레이어 1번

        // 직렬화
        var serializeObject = JsonConvert.SerializeObject(placeBet);

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
            aPIStorage.betting_id = jsonPlayer["data"]["betting_id"].ToString();
            Debug.Log("PostPlaveBetCaller Data Save Complited");

            StartCoroutine(WinnerCaller());
        }
    }

    // 이긴 사람이 나오면 이긴 사람의 id를 가지고 호출
    public IEnumerator WinnerCaller()
    {
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/zera/declare-winner";

        // 배팅 ID을 가져오고 이긴사람의 id
        WWWForm form = new WWWForm();

        winner winnerBet = new winner();
        winnerBet.betting_id = aPIStorage.betting_id;
        winnerBet.winner_player_id = aPIStorage.winner_id;
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
            aPIStorage.amount_won = jsonPlayer["data"]["amount_won"].ToString();
            Debug.Log("WinnerCaller Data Save Complited");
        }
    }
}



