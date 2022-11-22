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

    enum InfoState
    {
        Player,
        Tower,
        Minion,
        Nexus,
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
    public GameObject InfoPanel;
    public Image InfoIcon;
    public Image InfoHealthBar;
    public TextMeshProUGUI InfoHealthBarTMPro;
    public TextMeshProUGUI InfoDpsTMpro;
    public TextMeshProUGUI InfoSpdMpro;

    [Header("SkillUI")]
    public GameObject skillTable;

    [Header("ScoreUI")]
    public TextMeshProUGUI scoreTMPro;
    public TextMeshProUGUI timerTMPro;
    public static PlayerHUD Instance;

    [Header("GameResultUI")]
    public GameObject GameWinPanel;
    public Sprite GameWinSprite;
    public Sprite GameDefeatSprite;
    public GameObject GameResultImage;
    public Sprite GameResultWin;
    public Sprite GameResultDef;
    public Sprite GameResultDraw;
    private bool winPanel = false;

    [Header("GameESCUI")]
    public GameObject ESCButton;

    private Health playerHp;
    private Health enemyHp;

    [Header("ChatUI")]
    public TMP_InputField ChatInput;
    public GameObject ChatPanel;
    public TextMeshProUGUI[] ChatText;
    private bool ChatingUi = false;
    private bool Chating = false;


    [Header("MousePointer")]
    public Canvas MousePointerCanvas;
    public GameObject MousePositionImage1;
    public GameObject MousePositionImage2;
    public MousePointer mousePointer;

    [Header("BossMonsterUI")]
    public GameObject BossSpawnNotification;

    

    private int[] playerScores = { 0, 0 };

    float sec = 0;
    int min = 0;

    [SerializeField]
    public bool BossMonsterSpawnON { get; private set; }

    Player currentPlayerforInfo;
    Turret currentTurretforInfo;
    Enemybase currentMinionforInfo;
    NexusHp currentNexusforInfo;

    WaitForSeconds Delay500 = new WaitForSeconds(5);

    public string lastDamageTeam;



    IEnumerator imigeCourutine;
    IEnumerator textCouruntine;
    IEnumerator resultImigePopup;

    InfoState INFO;

    // 이동 일반 커서
    [SerializeField]
    public Texture2D cursorMoveNamal;
    // 이동 아군 커서
    [SerializeField]
    public Texture2D cursorMoveAlly;
    // 이동 적군 커서
    [SerializeField]
    public Texture2D cursorMoveEnemy;

    // 공격 일반 커서
    [SerializeField]
    public Texture2D cusorAttackNomal;
    // 공격 아군 커서
    [SerializeField]
    public Texture2D cusorAttackAlly;
    // 공격 적군 커서
    [SerializeField]
    public Texture2D cusorAttackEnemy;

    private void Awake()
    {
        Instance = this;
        Turret.turretMouseDownEvent += ActivationTowerInfoUI;
        Player.PlayerMouseDownEvent += ActivationEnemyInfoUI;
        Enemybase.minionMouseDownEvent += ActivationMinionInfoUI;
        NexusHp.nexusMouseDownEvent += ActivationNexusInfoUI;
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
        BossSpawnNotification.SetActive(false);
    }

    private void Start()
    {
        imigeCourutine = ImageFadeIn();
        resultImigePopup = ResultImagePopUp();
        textCouruntine = textFadeout();



        setSkill();
        StartCoroutine(setHp());
        setMouseCursor();
        photonView.RPC("RPCInitScore", RpcTarget.All);
        for (int i = 0; i < ChatText.Length; i++)
        {
            ChatText[i].text = "";
        }
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
        UpdateInfo();

        if (false == Chating && Input.GetKeyDown(KeyCode.Return))
        {
            ChatingUi = !ChatingUi; //온오프
            ChatPanel.SetActive(Chating); // 채팅창온
            if (ChatingUi && ChatInput.text == "")
            {
                ChatingUi = false;
            }


        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESCButton_S();
        }
    }


    #region ⋆⁺₊⋆ Skill Panel ⋆⁺₊⋆

    private void setSkill()
    {
        int count = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems.Count;

        for (int i = 0; i < count; i++)
        {
            Item item = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems[i];
            int slotIndex = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillIndex[i];
            skillTable.transform.GetChild(slotIndex).GetChild(0).gameObject.SetActive(true);
            skillTable.transform.GetChild(slotIndex).GetChild(0).GetComponent<Skillicon>().item = item;
            skillTable.transform.GetChild(slotIndex).GetComponent<SkillButton>().item = item;
            skillTable.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
        }
    }

    #endregion


    #region 🕦 Timer & Scroe Panel 🕦

    void Timer()
    {
        if ((int)sec < 0)
        {
            sec = 60;
            min--;
        }
        else if (GameManager.Instance.winner == "Draw")
        {
            if ((int)sec >= 60)
            {
                sec = 0;
                min++;
            }
            sec += Time.deltaTime;
            timerTMPro.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
            return;
        }


        sec -= Time.deltaTime;
        timerTMPro.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);

        if (min < 0)
        {
            string gameWinMessage = "";

            if (playerScores[(int)PlayerColor.Blue] == playerScores[(int)PlayerColor.Red])
            {
                GameManager.Instance.winner = "Draw";
            }
            else if (playerScores[(int)PlayerColor.Blue] > playerScores[(int)PlayerColor.Red])
            {
                GameManager.Instance.winner = "Blue";
                gameWinMessage = GameManager.Instance.winner;
                GameManager.Instance.isGameEnd = true;
            }
            else if ((playerScores[(int)PlayerColor.Blue] < playerScores[(int)PlayerColor.Red]))
            {
                GameManager.Instance.winner = "Red";
                gameWinMessage = GameManager.Instance.winner;
                GameManager.Instance.isGameEnd = true;
            }
            photonView.RPC("RPCInitScore", RpcTarget.All);

            //photonView.RPC("RPC_ActivationGameWinUI", RpcTarget.All, gameWinMessage);
            min = 0;
            sec = 0;
            timerTMPro.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);

            onGameEnd.Invoke();

            // 승패 이미지 호출
            StartCoroutine(resultImigePopup);

            //StartCoroutine(DelayLeaveRoom());
            return;
        }
    }

    public void AddScoreToEnemy(string tag)
    {
        if (GameManager.Instance.winner == "Draw")
        {
            return;
        }

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

    #endregion


    #region ⚙️ MainMenu Panel ⚙️

    private void ESCButton_S()
    {
        ESCButton.SetActive(true);
    }

    #endregion


    #region 🚩 GameResult Panel 🚩

    // 승패 결과 이미지 팝업 함수
    private IEnumerator ResultImagePopUp()
    {
        // 오브젝트 활성화
        GameWinPanel.SetActive(true);

        //GameManager.Instance.winner = "Blue";

        // 승자가 블루면
        if (GameManager.Instance.winner == "Draw")
        {

            if (PhotonNetwork.IsMasterClient)
            {
                GameWinPanel.GetComponent<Image>().sprite = GameResultDraw;
                FadeinImige();
                yield return Delay500;

                GameWinPanel.SetActive(false);
                BossSpawnNotification.SetActive(true);
                StartCoroutine(textCouruntine);


                ////GameResultImage.SetActive(true);
                GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultDraw;
            }
            else
            {
                GameWinPanel.GetComponent<Image>().sprite = GameResultDraw;
                FadeinImige();
                yield return Delay500;
                GameWinPanel.SetActive(false);
                BossSpawnNotification.SetActive(true);
                StartCoroutine(textCouruntine);

                //GameResultImage.SetActive(true);
                GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultDraw;
            }
            yield return null;
        }
        else if (GameManager.Instance.winner == "Blue")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameWinPanel.GetComponent<Image>().sprite = GameWinSprite;
                FadeinImige();

                yield return Delay500;
                GameWinPanel.SetActive(false);
                GameResultImage.SetActive(true);
                GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultWin;
            }
            else
            {
                GameWinPanel.GetComponent<Image>().sprite = GameDefeatSprite;
                FadeinImige();

                yield return Delay500;
                GameWinPanel.SetActive(false);
                GameResultImage.SetActive(true);
                GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultDef;
            }
        }
        else if (GameManager.Instance.winner == "Red")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameWinPanel.GetComponent<Image>().sprite = GameDefeatSprite;
                FadeinImige();

                yield return Delay500;
                GameWinPanel.SetActive(false);
                GameResultImage.SetActive(true);
                GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultDef;
            }
            else
            {
                GameWinPanel.GetComponent<Image>().sprite = GameWinSprite;
                FadeinImige();

                yield return Delay500;
                GameWinPanel.SetActive(false);
                GameResultImage.SetActive(true);
                GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultWin;
            }
        }
    }


    // 페이드 인 함수
    private IEnumerator ImageFadeIn()
    {
        float fadeValue = 0;
        Image gameWinPanel = GameWinPanel.GetComponent<Image>();
        while (fadeValue <= 1.0f)
        {
            fadeValue += 0.5f;


            yield return new WaitForSeconds(0.01f);

            gameWinPanel.color = new Color(255, 255, 255, fadeValue);

        }

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

        //GameManager.Instance.winner = "Blue";

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

        // 승패 이미지 호출
        StartCoroutine(ResultImagePopUp());

        //StartCoroutine(DelayLeaveRoom());
    }

    private IEnumerator DelayLeaveRoom()
    {
        yield return new WaitForSeconds(7f);
        PhotonNetwork.LeaveRoom();
    }

    #endregion


    #region 📢 Chatting Panel 📢
    public void Sned()
    {
        string msg = PhotonNetwork.LocalPlayer.ActorNumber + " : " + ChatInput.text;
        photonView.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    [PunRPC]
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)

            if (ChatText[i].text == "")
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


    #region 😁 Player Status Panel 😁

    private IEnumerator setHp()
    {
        yield return new WaitForSeconds(0.5f);
        playerHp = GameManager.Instance.CurrentPlayers[0].GetComponent<Health>();
        enemyHp = GameManager.Instance.CurrentPlayers[1].GetComponent<Health>();
        StopCoroutine(setHp());
    }

    float playerHp2D;
    void UpdateHealthUI()
    {
        if (playerHp == null)
        {
            return;
        }

        playerHealthBar.fillAmount = playerHp.hpSlider3D.value / playerHp.hpSlider3D.maxValue;
        playerHp2D = playerHp.hpSlider3D.value;
        playerHealthBarTMpro.text = (int)playerHp2D + " / " + playerHp.hpSlider3D.maxValue;
    }

    #endregion


    #region 🕮 Info Panel 🕮

    float Hp2D;
    public void UpdateInfo()
    {
        if (INFO == InfoState.Player)
        {
            if (enemyHp == null)
            {
                return;
            }

            if (currentPlayerforInfo == null)
            {
                return;
            }

            if (enemyHp.hpSlider3D.value <= 0)
            {
                InfoPanel.SetActive(false);
            }

            InfoHealthBar.fillAmount = enemyHp.hpSlider3D.value / enemyHp.hpSlider3D.maxValue;
            Hp2D = enemyHp.hpSlider3D.value;
            InfoHealthBarTMPro.text = (int)Hp2D + " / " + enemyHp.hpSlider3D.maxValue;

            float dmg = currentPlayerforInfo.playerStats.attackDmg;
            float atkSpeed = currentPlayerforInfo.playerStats.attackSpeed;
            float moveSpeed = currentPlayerforInfo.playerStats.MoveSpeed;
            float dps = dmg * atkSpeed;

            InfoDpsTMpro.text = dps.ToString();
            InfoSpdMpro.text = moveSpeed.ToString();
        }

        else if (INFO == InfoState.Tower)
        {
            if (currentTurretforInfo == null)
            {
                return;
            }

            if (currentTurretforInfo.currentHealth <= 0)
            {
                InfoPanel.SetActive(false);
            }

            // 실시간 체력 동기화
            InfoHealthBar.fillAmount = currentTurretforInfo.currentHealth / currentTurretforInfo.towerData.MaxHP;
            Hp2D = currentTurretforInfo.currentHealth;
            InfoHealthBarTMPro.text = (int)Hp2D + " / " + currentTurretforInfo.towerData.MaxHP;

            // 실시간 dps / speed 동기화
            float dmg = currentTurretforInfo.attack;
            float atkSpeed = currentTurretforInfo.attackSpeed;
            float dps = dmg * atkSpeed;
            InfoDpsTMpro.text = dps.ToString();
            InfoSpdMpro.text = 0.ToString();
        }

        else if (INFO == InfoState.Minion)
        {
            if (currentMinionforInfo == null)
            {
                return;
            }

            if (currentMinionforInfo.CurrnetHP <= 0)
            {
                InfoPanel.SetActive(false);
            }

            InfoHealthBar.fillAmount = currentMinionforInfo.CurrnetHP / currentMinionforInfo.HP;
            Hp2D = currentMinionforInfo.CurrnetHP;
            InfoHealthBarTMPro.text = (int)Hp2D + " / " + currentMinionforInfo.HP;
        }

        else if (INFO == InfoState.Nexus)
        {
            if (currentNexusforInfo == null)
            {
                return;
            }

            if (currentNexusforInfo.CurrentHp <= 0)
            {
                InfoPanel.SetActive(false);
            }

            InfoHealthBar.fillAmount = currentNexusforInfo.CurrentHp / currentNexusforInfo.MaxHp;
            Hp2D = currentNexusforInfo.CurrentHp;
            InfoHealthBarTMPro.text = (int)Hp2D + " / " + currentNexusforInfo.MaxHp;
        }


    }

    public void ActivationEnemyInfoUI(Player player)
    {
        INFO = InfoState.Player;
        currentPlayerforInfo = player;
        InfoPanel.SetActive(true);

        InfoIcon.sprite = player.playerIcon;
    }

    public void ActivationTowerInfoUI(Turret turret)
    {
        INFO = InfoState.Tower;
        currentTurretforInfo = turret;
        InfoPanel.SetActive(true);

        // 이벤트로 들어온 매개변수 세팅(Item class)
        InfoIcon.sprite = turret.towerData.Icon;
    }

    public void ActivationMinionInfoUI(Enemybase minion, Sprite icon)
    {
        INFO = InfoState.Minion;
        currentMinionforInfo = minion;
        InfoPanel.SetActive(true);

        InfoIcon.sprite = icon;
        float dmg = minion.Damage;
        float atkSpeed = minion.AttackSpeed;
        float spd = minion.moveSpeed;

        float dps = dmg * atkSpeed;
        InfoDpsTMpro.text = dps.ToString();
        InfoSpdMpro.text = spd.ToString();
    }

    public void ActivationNexusInfoUI(NexusHp nexus, Sprite icon)
    {
        INFO = InfoState.Nexus;
        currentNexusforInfo = nexus;
        InfoPanel.SetActive(true);

        InfoIcon.sprite = icon;
        InfoDpsTMpro.text = 0.ToString();
        InfoSpdMpro.text = 0.ToString();
    }

    #endregion


    #region 🔑 InGame System 🔑

    private void setMouseCursor()
    {
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMouseCanvas = MousePointerCanvas;
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMouseObj1 = MousePositionImage1;
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMouseObj2 = MousePositionImage2;
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMousePointer = mousePointer;
    }

    #endregion

    private void FadeinImige()
    {
        StartCoroutine(imigeCourutine);

    }

    private IEnumerator textFadeout()
    {
        float fadeValue = 1f;

        TextMeshProUGUI bossText = BossSpawnNotification.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        while (fadeValue >= 0f)
        {
            fadeValue -= 0.2f;
            if (fadeValue <= 0f)
            {
                BossMonsterSpawnON = true;
                GameManager.Instance.bossMonsterSpawn();
            }
            yield return new WaitForSeconds(0.1f);

            bossText.color = new Color(255, 255, 255, fadeValue);
        }
    }

    #region Report System

    [Header("ReportUI")]
    public GameObject ReportPanel;
    [SerializeField]
    private GameObject SuccessPanel;
    [SerializeField]
    private TextOverFlow textoverflow;
    private bool CheckboxOn = false;
    private  bool button = false;

    public void RefortButton()
    {
        button = !button;
        if (false == button)
        {
            ReportPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            ReportPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            textoverflow.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            textoverflow.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
            CheckboxOn = false;
            textoverflow.text.text = "";
        }
        ReportPanel.SetActive(button);
        textoverflow.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
    }


    public void Checkbox(int value)
    {
        CheckboxOn = true;
        if (value == 0)
        {
            ReportPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.yellow;
            ReportPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        }
        else
        {
            ReportPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            ReportPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.yellow;
        }


    }

    public void SendReport() // 2 입력안햇을때  3 체크박스 체크안햇을때  4 글자수 오바됬을때
    {
        Debug.Log($"{textoverflow.textNotEnter} : {CheckboxOn}");

        if (!textoverflow.textNotEnter && CheckboxOn) //체크박스 되있고 글자수 있을때
        {
            
            SuccessPanel.SetActive(true);
        }
        else if (textoverflow.textNotEnter == true) // 글자아무것도 안입력햇을때
        {
            

            textoverflow.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            textoverflow.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);

            // 텍스트 이미지 출력
            return;
        }
        else if (CheckboxOn == false)
        {

            
            textoverflow.gameObject.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            textoverflow.gameObject.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
            return;
        }
    }

    #endregion
}