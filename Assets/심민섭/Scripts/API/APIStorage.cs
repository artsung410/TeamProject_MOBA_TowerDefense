using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Place-bet
public struct placeBet
{
    public string[] players_session_id;
    public string bet_id;
}

public class MatchDetails
{
}

// Winner
public struct winner
{
    public string betting_id;
    public string winner_player_id;
    public MatchDetails match_details; // �� ��
}

// Disconnect
public struct disconnect
{
    public string betting_id;
}

public class APIStorage : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // ������ ������ 2�δ����� �Ѵ�.
    // [0] : Player1(Host), [1] : Player2(Client)
    private static APIStorage _instance;
    public static APIStorage Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(APIStorage)) as APIStorage;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int[] playerNumber = new int[2];

    public string apiKey = "5hO4J33kQPhtHhq4e0F76V";

    // ���� ���� �غ� ����
    public bool[] ready = new bool[2];

    // ������ ��ȭ�� ������ ����
    public string[] zera = new string[2];
    public string[] dappx = new string[2];
    // API ȣ�� ���� �ڵ� 200�� �ƴϸ� ������ ��ȯ�Ѵ�.
    public string[] statusCode = new string[2];

    // ���� ���� ID
    public string[] _id = new string[2];

    // ���� ���� �̸�
    public string[] userName = new string[2];

    // �Ź� �ٲ�� ���� ID
    public string[] session_id = new string[2];

    // Post �޼��� ȣ�� ��, "success"
    public string[] message = new string[2];

    // �¸��� ����
    public string amount_won;

    // ���ÿ� �ʿ��� ID
    public string[] bet_id = new string[2];

    // ������ �ߴٴ� ���� ������ ���� �ִ� ID
    public string betting_id;

    // ����� id
    public string winner_id = "633b86420e028f7ecb10fd09";

    // (�ӽ�) MetaMask _id(�׽�Ʈ �뵵)
    public string MetaMaskSessionID = "BZzdZUHXgnpUVHogHz5TpVfKf0OTqCTUuAEQBdeP";

}
