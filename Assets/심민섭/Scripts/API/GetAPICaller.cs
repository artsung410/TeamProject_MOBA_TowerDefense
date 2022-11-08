using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class GetAPICaller : MonoBehaviourPun
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // ------- 텍스트 출력 --------
    //private Text text;

    private string apiKey = "5hO4J33kQPhtHhq4e0F76V";

    [SerializeField]
    public PlayerStorage playerStorage;

    private static GetAPICaller _instance;

    public static GetAPICaller Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GetAPICaller)) as GetAPICaller;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    //public bool getAPIComplite = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);

        // 플레이어 숫자가 1이면 자신이 1번플레이어가 되는 거지
        if (playerStorage.playerNumber == -1)
        {
            playerStorage.playerNumber = PhotonNetwork.CountOfPlayers;
        }
        //Debug.Log(PhotonNetwork.CountOfPlayers);
        StartCoroutine(getUserProfileCaller());
    }

    //private void Start()
    //{
    //    DontDestroyOnLoad(gameObject);
    //    //text = GameObject.FindGameObjectWithTag("TestText").GetComponent<Text>();
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //text.text = $"_id : {playerStorage._id}\nseid : {playerStorage.session_id}\nzera : {playerStorage.zera}\nace : {playerStorage.ace}\nplayerNumber : {playerStorage.playerNumber}";
            /*GameObject playerQueueObj = GameObject.FindGameObjectWithTag("PlayerQueue").gameObject;
            PlayerQueue palyerQueue = playerQueueObj.GetComponent<PlayerQueue>();

            for (int i = 0; i < palyerQueue.queue.Count; i++)
            {
                if (true == palyerQueue.queue.Contains(-1))
                {
                    palyerQueue.queue.Dequeue();
                    palyerQueue.queue.Enqueue(playerStorage.userName);
                    playerStorage.playerNumber = i;
                    return;
                }
                else if (false == palyerQueue.queue.Contains(-1))
                {
                    palyerQueue.queue.Enqueue(playerStorage.userName);
                    playerStorage.playerNumber = i + 1;
                    return;
                }
            }*/

            // 플레이어 닉네임을 넣는다.
            //aPIStorage.queue.Enqueue(playerStorage.userName);
            /*for (int i = 0; i < aPIStorage.queue.Count; i++)
            {
                if (true == aPIStorage.queue.Contains(playerStorage.userName))
                {
                    playerStorage.playerNumber = i;
                    getAPIComplite = false;
                    return;
                }
            }*/
        }

        /*if (getAPIComplite)
        {
            GameObject apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
            APIStorage aPIStorage = apiStorageObj.GetComponent<APIStorage>();

            // 플레이어 닉네임을 넣는다.
            aPIStorage.queue.Enqueue(playerStorage.userName);
            for (int i = 0; i < aPIStorage.queue.Count; i++)
            {
                if (true == aPIStorage.queue.Contains(playerStorage.userName))
                {
                    playerStorage.playerNumber = i;
                    getAPIComplite = false;
                    return;
                }
            }
        }*/
    }

    // 호출 정보 : StatusCode, _id, username
    private IEnumerator getUserProfileCaller()
    {
        string getUserProfile = "http://localhost:8546/api/getuserprofile";
        using (UnityWebRequest www = UnityWebRequest.Get(getUserProfile))
        {
            yield return www.SendWebRequest();
            // HTTP 에러 디버그
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // 데이터 저장
            //playerStorage.statusCode = jsonPlayer["StatusCode"].ToString();
            playerStorage.userName = jsonPlayer["userProfile"]["username"].ToString();
            playerStorage._id = jsonPlayer["userProfile"]["_id"].ToString();

            /*GameObject apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
            APIStorage aPIStorage = apiStorageObj.GetComponent<APIStorage>();
            aPIStorage.userName[playerStorage.playerNumber] = jsonPlayer["userProfile"]["username"].ToString();
            aPIStorage._id[playerStorage.playerNumber] = jsonPlayer["userProfile"]["_id"].ToString();*/
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getSessionIDCaller());
        }
    }
    // 호출 정보 : StatusCode, sessionId
    private IEnumerator getSessionIDCaller()
    {
        string getSessionID = "http://localhost:8546/api/getsessionid";
        using (UnityWebRequest www = UnityWebRequest.Get(getSessionID))
        {
            yield return www.SendWebRequest();

            // HTTP 에러 디버그
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // 데이터 저장
            //playerAPIInfoDB.statusCode = jsonPlayer["StatusCode"].ToString();
            playerStorage.session_id = jsonPlayer["sessionId"].ToString();
            /*GameObject apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
            APIStorage aPIStorage = apiStorageObj.GetComponent<APIStorage>();
            aPIStorage.session_id[playerStorage.playerNumber] = jsonPlayer["sessionId"].ToString();*/
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getbettingCurrencyCaller());
        }
    }
    // 호출 정보 : message, data{balance}
    public IEnumerator getbettingCurrencyCaller()
    {

        // Zera
        string getbettingCurrencyZera = $"https://odin-api-sat.browseosiris.com/v1/betting/zera/balance/{playerStorage.session_id}";
        //string getbettingCurrency = $"https://odin-api-sat.browseosiris.com/v1/betting/{storage.currency}/balance/{storage.sessionId}";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrencyZera))
        {
            yield return www.SendWebRequest();

            // HTTP 에러 디버그
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // 데이터 저장
            //playerStorage.message = jsonPlayer["message"].ToString();
            playerStorage.zera = jsonPlayer["data"]["balance"].ToString();
            string zeraValue = $"{float.Parse(playerStorage.zera): 0}";
            GameObject.FindGameObjectWithTag("Zera").GetComponent<Text>().text = zeraValue;
            /*GameObject apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
            APIStorage aPIStorage = apiStorageObj.GetComponent<APIStorage>();
            aPIStorage.zera[playerStorage.playerNumber] = jsonPlayer["data"]["balance"].ToString();*/
            Debug.Log("getbettingCurrency Zera Data Save Complited");
            StartCoroutine(getSettingsCaller());
        }

        // Ace
        string getbettingCurrencyAce = $"https://odin-api-sat.browseosiris.com/v1/betting/ace/balance/{playerStorage.session_id}";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrencyAce))
        {
            yield return www.SendWebRequest();

            // HTTP 에러 디버그
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // 데이터 저장
            playerStorage.ace = jsonPlayer["data"]["balance"].ToString();
            string aceValue = $"{float.Parse(playerStorage.ace): 0}";
            GameObject.FindGameObjectWithTag("Dappx").GetComponent<Text>().text = aceValue;

            /*GameObject apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
            APIStorage aPIStorage = apiStorageObj.GetComponent<APIStorage>();
            aPIStorage.ace[playerStorage.playerNumber] = jsonPlayer["data"]["balance"].ToString();*/
            Debug.Log("getbettingCurrency Ace Data Save Complited");
            //getAPIComplite = true;
        }
    }

    // 호출 정보 : message, data{balance}
    public IEnumerator getSettingsCaller()
    {
        string getbettingCurrency = $"https://odin-api-sat.browseosiris.com/v1/betting/settings";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrency))
        {
            www.SetRequestHeader("api-key", apiKey);
            yield return www.SendWebRequest();

            // HTTP 에러 디버그
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);
            // 데이터 저장
            playerStorage.bet_id = jsonPlayer["data"]["bets"][0]["_id"].ToString();
            Debug.Log("getSettingsCaller Data Save Complited");
        }
    }
}