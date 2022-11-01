using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHUD : MonoBehaviourPun
{
    public static event Action onGameEnd = delegate { };
    enum Player
    {
        Blue,
        Red,
    }

    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    [Header("PlayerStatusUI")]
    public Image playerHealthBar;
    public Image playerExperienceBar;
    public TextMeshProUGUI playerHealthBarTMpro;
    public TextMeshProUGUI playerExperienceBarTMpro;
    public TextMeshProUGUI playerInfoTMPro;

    [Header("InfoUI")]
    public Image EnemyHealthBar;
    public TextMeshProUGUI enemyHealthBarTMPro;

    [Header("SkillUI")]
    public GameObject skillTable;

    [Header("ScoreUI")]
    public TextMeshProUGUI scoreTMPro;
    public TextMeshProUGUI timerTMPro;
    public static PlayerHUD Instance;

    [Header("GameWinUI")]
    public GameObject GameWinPanel;
    public TextMeshProUGUI GameWinText;

    private Health playerHp;
    private Health enemyHp;

    [Header("ChatUI")]
    public TMP_InputField ChatInput;
    public GameObject ChatPanel;
    public TextMeshProUGUI[] ChatText;
    



    [Header("MousePointer")]
    public Canvas MousePointerCanvas;
    public GameObject MousePositionImage;
    public MousePointer mousePointer;

    //[Header("WinnerResultImage")]
    //public GameObject gameWinImagePanel;
    //public Image blueWinImage;
    //public Image redWinImage;

    private int[] playerScores = { 0, 0 };

    float sec = 0;
    int min = 0;

    private bool Chating = false;

    private void Awake()
    {
        Instance = this;

        ChatPanel.SetActive(false);
    }

    private void OnEnable()
    {
        // 게임 제한 시간 관련 코드
        TrojanHorse tro = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>();
        int time = tro.limitedTime;

        //시, 분, 초 선언
        int hours, minute, second;

        //시간공식
        hours = time / 3600;
        minute = time % 3600 / 60;
        second = time % 3600 % 60;
        min = minute;
        sec = second;
    }

    private void Start()
    {
        setSkill();
        StartCoroutine(setHp());
        setMouseCursor();
        photonView.RPC("RPCInitScore", RpcTarget.All);
        for (int i = 0;  i < ChatText.Length; i++)
        {
            ChatText[i].text = "";
        }
    }

    private IEnumerator setHp()
    {
        yield return new WaitForSeconds(0.5f);
        playerHp = GameManager.Instance.CurrentPlayers[0].GetComponent<Health>();
        enemyHp = GameManager.Instance.CurrentPlayers[1].GetComponent<Health>();
        StopCoroutine(setHp());
    }

    private void setSkill()
    {
        int count = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems.Count;

        for (int i = 0; i < count; i++)
        {
            Item item = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems[i];
            skillTable.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            skillTable.transform.GetChild(i).GetChild(0).GetComponent<Skillicon>().item = item;
            skillTable.transform.GetChild(i).GetComponent<SkillButton>().item = item;
            skillTable.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
        }
    }

    private void setMouseCursor()
    {
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMouseCanvas = MousePointerCanvas;
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMouseObj = MousePositionImage;
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMousePointer = mousePointer;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }
        
        Timer();
    }

    void Update()
    {
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {

            ChatOn(); //채팅창온 
            Debug.Log("키입력받나요??");
        }

        UpdateHealthUI();
        UpdateEnemyHealthUI();
    }
    

   public void ChatOn()
    {
        Chating = !Chating;
        ChatPanel.SetActive(Chating);
    }


    void Timer()
    {
        if ((int)sec < 0)
        {
            sec = 60;
            min--;
        }

        sec -= Time.deltaTime;
        timerTMPro.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);

        if (min < 0)
        {
            string gameWinMessage = "";

            if (playerScores[(int)Player.Blue] > playerScores[(int)Player.Red])
            {
                GameManager.Instance.winner = "Blue";
                gameWinMessage = GameManager.Instance.winner;
            }
            else if ((playerScores[(int)Player.Blue] < playerScores[(int)Player.Red]))
            {
                GameManager.Instance.winner = "Red";
                gameWinMessage = GameManager.Instance.winner;
            }
            else
            {
                gameWinMessage = "Draw";
                photonView.RPC("RPCInitScore", RpcTarget.All);
            }

            photonView.RPC("RPC_ActivationGameWinUI", RpcTarget.All, gameWinMessage);
            min = 0;
            sec = 0;
            timerTMPro.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
            GameManager.Instance.isGameEnd = true;
            onGameEnd.Invoke();
            StartCoroutine(DelayLeaveRoom());
            return;
        }
    }

    float playerHp2D; float EnemyHp2D;
    void UpdateHealthUI()
    {
        if (playerHp == null)
        {
            return;
        }

        playerHealthBar.fillAmount = playerHp.hpSlider3D.value / playerHp.hpSlider3D.maxValue;
        playerHp2D = playerHp.hpSlider3D.value;
        playerHealthBarTMpro.text = playerHp2D + " / " + playerHp.hpSlider3D.maxValue;
    }

    void UpdateEnemyHealthUI()
    {
        if (enemyHp == null)
        {
            return;
        }

        EnemyHealthBar.fillAmount = enemyHp.hpSlider3D.value / enemyHp.hpSlider3D.maxValue;
        EnemyHp2D = enemyHp.hpSlider3D.value;
        enemyHealthBarTMPro.text = EnemyHp2D + " / " + enemyHp.hpSlider3D.maxValue;
    }

    public void AddScoreToEnemy(string tag)
    {
        if (tag == "Red")
        {
            playerScores[(int)Player.Blue] += 1;
        }
        else
        {
            playerScores[(int)Player.Red] += 1;
        }

        // RpcTarget : 어떤 클라이언트에게 동기화를 징행할 것인지, All이면 모든 클라이언트들에게 동기화 진행.
        photonView.RPC("RPCUpdateScoreText", RpcTarget.All, playerScores[0].ToString(), playerScores[1].ToString());
    }

    [PunRPC]
    private void RPCUpdateScoreText(string player1ScoreText, string player2ScoreText)
    {
        scoreTMPro.text = $"{player1ScoreText}        {player2ScoreText}";
    }

    [PunRPC]
    private void RPCInitScore()
    {
        scoreTMPro.text = $"0        0";
    }

    public void ActivationGameWinUI_Nexus(string tag)
    {
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

        GameManager.Instance.isGameEnd = true;
        onGameEnd.Invoke();

        string gameWinMessage = "";

        if (tag == "Red")
        {
            GameManager.Instance.winner = "Blue";
            gameWinMessage = GameManager.Instance.winner;
        }
        else
        {
            GameManager.Instance.winner = "Red";
            gameWinMessage = GameManager.Instance.winner;
        }

        photonView.RPC("RPC_ActivationGameWinUI", RpcTarget.All, GameManager.Instance.winner);
        StartCoroutine(DelayLeaveRoom());
    }

    [PunRPC]
    private void RPC_ActivationGameWinUI(string message)
    {
        if (message == "Draw")
        {
            GameWinText.text = "Draw!";
        }
        else
        {
            GameWinText.text = $"{message} Team Game Win!";
        }

        GameWinPanel.SetActive(true);
    }

    private IEnumerator DelayLeaveRoom()
    {
        yield return new WaitForSeconds(7f);
        PhotonNetwork.LeaveRoom();
    }

    #region 채팅
    public void Sned()
    {
        string msg = PhotonNetwork.LocalPlayer.ActorNumber + " : " + ChatInput.text;
        photonView.RPC("ChatRPC",RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    [PunRPC]
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i <ChatText.Length; i++)
        
            if(ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;

            }
        
        if (!isInput)
            {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
            }
    }

    #endregion
}
