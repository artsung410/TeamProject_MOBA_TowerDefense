using LitJson;
using Newtonsoft.Json;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PostPlaveBetCaller : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private APIStorage aPIStorage;

    // 배팅 API 매칭
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //aPIStorage = GameObject.FindGameObjectWithTag("APIStorage").GetComponent<APIStorage>();
            StartCoroutine(PostPlaveBetCaller_S());
        }
        else
        {
            return;
        }
    }

    // 호출 정보 : message, betting_id
    public IEnumerator PostPlaveBetCaller_S()
    {
        yield return new WaitForSeconds(0.5f);
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/dappx/place-bet";

        // 여기선 두명의 세션 아이디를 가져와야함.
        WWWForm form = new WWWForm();
        aPIStorage = GameObject.FindGameObjectWithTag("APIStorage").GetComponent<APIStorage>();
        placeBet placeBet = new placeBet();
        placeBet.players_session_id = new string[2];
        placeBet.players_session_id[0] = aPIStorage.session_id[0];
        //placeBet.players_session_id[1] = aPIStorage.MetaMaskSessionID;
        placeBet.players_session_id[1] = aPIStorage.session_id[1];
        placeBet.bet_id = aPIStorage.bet_id[0];

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
        }
    }



}
