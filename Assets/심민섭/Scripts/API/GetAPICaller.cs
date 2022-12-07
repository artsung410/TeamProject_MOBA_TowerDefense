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

    // ------- �ؽ�Ʈ ��� --------
    //private Text text;

    private string apiKey = "5hO4J33kQPhtHhq4e0F76V";

    [SerializeField]
    private PlayerStorage playerStorage;

    private static GetAPICaller _instance;
    

    public static GetAPICaller Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GetAPICaller)) as GetAPICaller;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    //public bool getAPIComplite = false;

    private void Awake()
    {
        //CSVtest.onDataParsingEvent += DbInit;

        if (_instance == null)
        {
            _instance = this;
        }

        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            //Destroy(gameObject);
        }

        // �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject);

        // �÷��̾� ���ڰ� 1�̸� �ڽ��� 1���÷��̾ �Ǵ� ����
        if (playerStorage.playerNumber == -1)
        {
            playerStorage.playerNumber = PhotonNetwork.CountOfPlayers;
        }
        //Debug.Log(PhotonNetwork.CountOfPlayers);

        // API ȣ��, DB ������
        StartCoroutine(getUserProfileCaller());

    }

    /*public void DbInit()
    {
        StartCoroutine(getUserProfileCaller());
    }*/

    // ȣ�� ���� : StatusCode, _id, username
    private IEnumerator getUserProfileCaller()
    {
        string getUserProfile = "http://localhost:8546/api/getuserprofile";
        using (UnityWebRequest www = UnityWebRequest.Get(getUserProfile))
        {
            yield return www.SendWebRequest();
            // HTTP ���� �����
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // ������ ����
            //playerStorage.statusCode = jsonPlayer["StatusCode"].ToString();
            playerStorage.userName = jsonPlayer["userProfile"]["username"].ToString();
            playerStorage._id = jsonPlayer["userProfile"]["_id"].ToString();
            if (playerStorage._id != "")
            {
                DataBaseHandler.instance.USER_INIT_INFO_INSERT();
            }
            /*GameObject apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
            APIStorage aPIStorage = apiStorageObj.GetComponent<APIStorage>();
            aPIStorage.userName[playerStorage.playerNumber] = jsonPlayer["userProfile"]["username"].ToString();
            aPIStorage._id[playerStorage.playerNumber] = jsonPlayer["userProfile"]["_id"].ToString();*/
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getSessionIDCaller());
        }
    }
    // ȣ�� ���� : StatusCode, sessionId
    private IEnumerator getSessionIDCaller()
    {
        string getSessionID = "http://localhost:8546/api/getsessionid";
        using (UnityWebRequest www = UnityWebRequest.Get(getSessionID))
        {
            yield return www.SendWebRequest();

            // HTTP ���� �����
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // ������ ����
            //playerAPIInfoDB.statusCode = jsonPlayer["StatusCode"].ToString();
            playerStorage.session_id = jsonPlayer["sessionId"].ToString();
            /*GameObject apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
            APIStorage aPIStorage = apiStorageObj.GetComponent<APIStorage>();
            aPIStorage.session_id[playerStorage.playerNumber] = jsonPlayer["sessionId"].ToString();*/
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getbettingCurrencyCaller());
        }
    }
    // ȣ�� ���� : message, data{balance}
    public IEnumerator getbettingCurrencyCaller()
    {

        // Zera
        string getbettingCurrencyZera = $"https://odin-api-sat.browseosiris.com/v1/betting/zera/balance/{playerStorage.session_id}";
        //string getbettingCurrency = $"https://odin-api-sat.browseosiris.com/v1/betting/{storage.currency}/balance/{storage.sessionId}";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrencyZera))
        {
            yield return www.SendWebRequest();

            // HTTP ���� �����
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // ������ ����
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

        // Dappx
        string getbettingCurrencyDappx = $"https://odin-api-sat.browseosiris.com/v1/betting/dappx/balance/{playerStorage.session_id}";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrencyDappx))
        {
            yield return www.SendWebRequest();

            // HTTP ���� �����
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // ������ ����
            playerStorage.dappx = jsonPlayer["data"]["balance"].ToString();
            string dappxValue = $"{float.Parse(playerStorage.dappx): 0}";
            GameObject.FindGameObjectWithTag("Dappx").GetComponent<Text>().text = dappxValue;

            /*GameObject apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
            APIStorage aPIStorage = apiStorageObj.GetComponent<APIStorage>();
            aPIStorage.dappx[playerStorage.playerNumber] = jsonPlayer["data"]["balance"].ToString();*/
            Debug.Log("getbettingCurrency Dappx Data Save Complited");
            //getAPIComplite = true;
        }
    }

    // ȣ�� ���� : message, data{balance}
    public IEnumerator getSettingsCaller()
    {
        string getbettingCurrency = $"https://odin-api-sat.browseosiris.com/v1/betting/settings";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrency))
        {
            www.SetRequestHeader("api-key", apiKey);
            yield return www.SendWebRequest();

            // HTTP ���� �����
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);
            // ������ ����
            playerStorage.bet_id = jsonPlayer["data"]["bets"][0]["_id"].ToString();
            string winAmount = $"{float.Parse(jsonPlayer["data"]["bets"][0]["win_amount"].ToString()):0}";
            playerStorage.win_amount = winAmount;
            Debug.Log("getSettingsCaller Data Save Complited");
        }
    }
}