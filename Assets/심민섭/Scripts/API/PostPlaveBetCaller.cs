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

    // ���� API ��Ī
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

    // ȣ�� ���� : message, betting_id
    public IEnumerator PostPlaveBetCaller_S()
    {
        yield return new WaitForSeconds(0.5f);
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/dappx/place-bet";

        // ���⼱ �θ��� ���� ���̵� �����;���.
        WWWForm form = new WWWForm();
        aPIStorage = GameObject.FindGameObjectWithTag("APIStorage").GetComponent<APIStorage>();
        placeBet placeBet = new placeBet();
        placeBet.players_session_id = new string[2];
        placeBet.players_session_id[0] = aPIStorage.session_id[0];
        //placeBet.players_session_id[1] = aPIStorage.MetaMaskSessionID;
        placeBet.players_session_id[1] = aPIStorage.session_id[1];
        placeBet.bet_id = aPIStorage.bet_id[0];

        // ����ȭ
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

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);
            // ������ ����
            aPIStorage.betting_id = jsonPlayer["data"]["betting_id"].ToString();
            Debug.Log("PostPlaveBetCaller Data Save Complited");
        }
    }



}
