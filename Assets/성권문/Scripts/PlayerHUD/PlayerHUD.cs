using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHUD : MonoBehaviourPun
{
    public static event Action onGameEnd = delegate { };
    enum PlayerColor
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
                
    [Header("EnemyInfoUI")]
    public GameObject enemyInfoPanel;
    public Image enemyHealthBar;
    public TextMeshProUGUI enemyHealthBarTMPro;
    public TextMeshProUGUI enemyInfoTMPro;
    public TextMeshProUGUI enemyAtkTMpro;
    public TextMeshProUGUI enemyAtkSpdTMpro;
    public TextMeshProUGUI enemyArTMpro;
    public TextMeshProUGUI enemySpdMpro;


    [Header("TowerInfoUI")]
    public GameObject towerInfoPanel;
    public Image towerImage;
    public TextMeshProUGUI towerLevelTMPro;
    public TextMeshProUGUI towerInfoTMPro;

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

    private int[] playerScores = { 0, 0 };

    float sec = 0;
    int min = 0;

    private void Awake()
    {
        Instance = this;
        Turret.turretMouseDownEvent += ActivationTowerInfoUI;
        Player.PlayerMouseDownEvent += ActivationEnemyInfoUI;
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

        UpdateHealthUI();
        UpdateEnemyHealthUI();
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

            if (playerScores[(int)PlayerColor.Blue] > playerScores[(int)PlayerColor.Red])
            {
                GameManager.Instance.winner = "Blue";
                gameWinMessage = GameManager.Instance.winner;
            }
            else if ((playerScores[(int)PlayerColor.Blue] < playerScores[(int)PlayerColor.Red]))
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

        enemyHealthBar.fillAmount = enemyHp.hpSlider3D.value / enemyHp.hpSlider3D.maxValue;
        EnemyHp2D = enemyHp.hpSlider3D.value;
        enemyHealthBarTMPro.text = EnemyHp2D + " / " + enemyHp.hpSlider3D.maxValue;
    }

    public void AddScoreToEnemy(string tag)
    {
        if (tag == "Red")
        {
            playerScores[(int)PlayerColor.Blue] += 1;
        }
        else
        {
            playerScores[(int)PlayerColor.Red] += 1;
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

    public void ActivationEnemyInfoUI(Stats st)
    {
        DeActivationInfoUI_All();
        enemyInfoPanel.SetActive(true);

        float dmg = st.attackDmg;
        float atkSpeed = st.attackSpeed;
        float range = st.attackRange;

        enemyAtkTMpro.text = dmg.ToString();
        enemyAtkSpdTMpro.text = atkSpeed.ToString();
        enemyArTMpro.text = range.ToString();
    }

    public void ActivationTowerInfoUI(Item item, string tag)
    {
        DeActivationInfoUI_All();
        towerInfoPanel.SetActive(true);

        // 태그에 따라 색상 설정
        if (tag == "Blue")
        {
            towerInfoPanel.GetComponent<Image>().color = new Color(0f, 0f, 255f, 0.7f);
        }
        else
        {
            towerInfoPanel.GetComponent<Image>().color = new Color(255f, 0f, 0f, 0.7f);
        }

        // 이벤트로 들어온 매개변수 세팅(Item class)
        float hp = item.itemAttributes[0].attributeValue;
        float dmg = item.itemAttributes[1].attributeValue;
        float range = item.itemAttributes[2].attributeValue;
        towerImage.sprite = item.itemIcon;
        towerInfoTMPro.text = $"HP : {hp} / ATK : {dmg} / AR : {range}";
    }

    public void DeActivationInfoUI_All()
    {
        enemyInfoPanel.SetActive(false);
        towerInfoPanel.SetActive(false);
    }
}
