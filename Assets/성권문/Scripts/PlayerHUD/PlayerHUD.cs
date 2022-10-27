using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHUD : MonoBehaviourPun
{
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

    [Header("MousePointer")]
    public Canvas MousePointerCanvas;
    public GameObject MousePositionImage;
    public MousePointer mousePointer;

    //[Header("WinnerResultImage")]
    //public GameObject gameWinImagePanel;
    //public Image blueWinImage;
    //public Image redWinImage;

    private int[] playerScores = { 0, 0 };
    public bool isGameEnd;
    public string winner;
    float sec = 0;
    int min = 0;

    private void Awake()
    {
        Instance = this;
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
        if (isGameEnd == false)
        {
            Timer();
        }
    }

    void Update()
    {
        UpdateHealthUI();
        UpdateEnemyHealthUI();
    }

    void Timer()
    {
        if ((int)sec < 0)
        {
            sec = 59;
            min--;
        }

        sec -= Time.deltaTime;
        timerTMPro.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);

        if (min < 0)
        {
            string gameWinMessage = "";

            if (playerScores[(int)Player.Blue] > playerScores[(int)Player.Red])
            {
                winner = "Blue";
                gameWinMessage = "Blue Team Win!";
                StartCoroutine(DelayToTimeScale());
            }
            else if ((playerScores[(int)Player.Blue] < playerScores[(int)Player.Red]))
            {
                winner = "Red";
                gameWinMessage = "Red Team Win!";
                StartCoroutine(DelayToTimeScale());
            }
            else
            {
                gameWinMessage = "Draw!";
                photonView.RPC("RPCInitScore", RpcTarget.All);
                DelayDeActivationGameWinUI();
            }

            photonView.RPC("RPC_ActivationGameWinUI", RpcTarget.All, gameWinMessage);
            min = 0;
            sec = 0;
            timerTMPro.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
            isGameEnd = true;
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
        isGameEnd = true;
        string gameWinMessage = "";

        if (tag == "Red")
        {
            winner = "Blue";
            gameWinMessage = "Blue Team Win!";
        }
        else
        {
            winner = "Red";
            gameWinMessage = "Red Team Win!";
        }

        photonView.RPC("RPC_ActivationGameWinUI", RpcTarget.All, gameWinMessage);
        StartCoroutine(DelayToTimeScale());
    }

    [PunRPC]
    private void RPC_ActivationGameWinUI(string message)
    {
        GameWinText.text = message;
        GameWinPanel.SetActive(true);
    }

    private IEnumerator DelayDeActivationGameWinUI()
    {
        yield return new WaitForSeconds(2f);
        GameWinPanel.SetActive(false);
    }

    private IEnumerator DelayToTimeScale()
    {
        yield return new WaitForSeconds(1.5f);
        //Time.timeScale = 0;
    }

    private void DeActivationGameWinUI()
    {
        GameWinPanel.SetActive(false);
    }

}
