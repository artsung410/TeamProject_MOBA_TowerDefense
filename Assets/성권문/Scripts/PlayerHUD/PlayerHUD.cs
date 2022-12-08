#define JM_VER
//#define HS_VER

using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHUD : MonoBehaviourPun, IPunObservable
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
    public GameObject PlayerObject;
    private Stats playerStat;
    private Player playerScript;
    public Image PlayerPortrait;                        // 초상화
    public Image playerHealthBar;                       // 체력 게이지
    public Image playerExperienceBar;                   // 경험치 게이지
    public TextMeshProUGUI playerHealthBarTMpro;        // 체력 text
    public TextMeshProUGUI playerExperienceBarTMpro;    // 경험치 text
    public TextMeshProUGUI PlayerLevelText;             // 레벨 text
    public Text PlayerDpsText;                          // 초당공격력 text
    public Text PlayerSpeedText;                        // 이동속도 text

    [Header("InfoUI")]
    public GameObject InfoPanel;
    public Image InfoIcon;
    public Image InfoHealthBar;
    public TextMeshProUGUI InfoHealthBarTMPro;
    public TextMeshProUGUI InfoDpsTMpro;
    public TextMeshProUGUI InfoSpdMpro;
    public TextMeshProUGUI InfoLevel;

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

    [HideInInspector]
    public float sec = 0;
    [HideInInspector]
    public int min = 0;

    [SerializeField]
    public bool BossMonsterSpawnON { get; private set; }

    Player currentPlayerforInfo;
    Turret currentTurretforInfo;
    Enemybase currentMinionforInfo;
    NexusHp currentNexusforInfo;

    WaitForSeconds Delay500 = new WaitForSeconds(5);

    public string lastDamageTeam;
    public bool NeutalMonsterDie = false;



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
        int time = 600;

        //시, 분, 초 선언
        int hours, minute, second;

        //시간공식
        hours = time / 3600;
        minute = time % 3600 / 60;
        second = time % 3600 % 60;
        min = minute;
        sec = second;
        BossSpawnNotification.SetActive(false);


        bossText = BossSpawnNotification.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        imigeCourutine = ImageFadeIn();
        //resultImigePopup = ResultImagePopUp();
        textCouruntine = textFadeout();

        StartCoroutine(SetPlayer());
        setSkill();
        StartCoroutine(setHp());
        setMouseCursor();
        //photonView.RPC("RPCInitScore", RpcTarget.All);
        for (int i = 0; i < ChatText.Length; i++)
        {
            ChatText[i].text = "";
        }
    }

    float testTime;

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (GameWinPanel.GetComponent<Image>().sprite == GameResultDraw)
            {
                testTime += Time.deltaTime;
                if (testTime >= 1f)
                {
                    if ((int)sec >= 60)
                    {
                        sec = 0;
                        min++;
                    }
                    sec += Time.deltaTime;
                }
            }
            else
            {
                if ((int)sec < 0) // 1 
                {
                    sec = 60;
                    min--;
                }
                sec -= Time.deltaTime;

                if (min <= 0 && sec <= 0)
                {
                    min = 0;
                    sec = 0;
                    isTimeEnd = true;
                }
            }

        }

        // 시간을 이용한 구현부분
        Timer(min, sec, isTimeEnd);
    }

    void Update()
    {
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

        UpdateStatusUI();
        UpdateHealthUI();
        UpdateInfo();
        UpdateScore();

        if (false == Chating && Input.GetKeyDown(KeyCode.Return))
        {
            ChatingUi = !ChatingUi; //온오프
            ChatPanel.SetActive(Chating); // 채팅창온
            if (ChatingUi && ChatInput.text == "")
            {
                ChatingUi = false;
            }

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
            skillTable.transform.GetChild(slotIndex).GetChild(0).GetComponent<Image>().sprite = SkillManager.Instance.Datas[i].SkillIcon;
        }
    }

    #endregion


    #region 🕦 Timer & Scroe Panel 🕦

    public bool isTimeEnd;
    float winnerTime;
    float neutralTime;
    void Timer(int min, float sec, bool end)
    {
        timerTMPro.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
        if (NeutalMonsterDie)
        {
            neutralTime += Time.deltaTime;
            Debug.Log("시작");
            Debug.Log($"{GameManager.Instance.winner}");
            onGameEnd.Invoke();
            ResultPopUp(neutralTime, GameManager.Instance.winner);
            return;
        }

        // 호스트는 조건문이 실행, 클라이언트는 실행안됨
        if (end == true) // 3
        {
            string gameWinMessage = "";
            Debug.Log("time's up");
            winnerTime += Time.deltaTime;
            if (playerScores[(int)PlayerColor.Blue] == playerScores[(int)PlayerColor.Red])
            {
                Debug.Log("&&&&&&&&&&&&&& Draw &&&&&&&&&&&&&&&");
                GameManager.Instance.winner = "Draw";
                Debug.Log(GameManager.Instance.winner);
                gameWinMessage = GameManager.Instance.winner;
                ResultPopUp(winnerTime, gameWinMessage);
                return;
            }
            else if (playerScores[(int)PlayerColor.Blue] > playerScores[(int)PlayerColor.Red])
            {
                Debug.Log("&&&&&&&&&&&&&& Blue &&&&&&&&&&&&&&&");
                GameManager.Instance.winner = "Blue";
                gameWinMessage = GameManager.Instance.winner;
                Debug.Log("Image Call");
                //StartCoroutine(resultImigePopup);
                ResultPopUp(winnerTime, gameWinMessage);
                GameManager.Instance.isGameEnd = true;
            }
            else if ((playerScores[(int)PlayerColor.Blue] < playerScores[(int)PlayerColor.Red]))
            {
                Debug.Log("&&&&&&&&&&&&&& Red &&&&&&&&&&&&&&&");
                GameManager.Instance.winner = "Red";
                gameWinMessage = GameManager.Instance.winner;
                Debug.Log("Image Call");
                ResultPopUp(winnerTime, gameWinMessage);
                GameManager.Instance.isGameEnd = true;
            }

            //photonView.RPC(nameof(RPCInitScore), RpcTarget.All); 왜 필요한가?
            onGameEnd.Invoke();

        }
    }

    // 호스트가 관리하는 부분 : 시간, 게임종료여부, 점수
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            // We own this player: send the others our data
            stream.SendNext(sec);
            stream.SendNext(min);
            stream.SendNext(isTimeEnd);
            stream.SendNext(playerScores);
        }
        else if (stream.IsReading)
        {
            // Network player, receive data
            this.sec = (float)stream.ReceiveNext();
            this.min = (int)stream.ReceiveNext();
            this.isTimeEnd = (bool)stream.ReceiveNext();
            this.playerScores = (int[])stream.ReceiveNext();
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

        //photonView.RPC(nameof(RPCUpdateScoreText), RpcTarget.All, playerScores[0].ToString(), playerScores[1].ToString());
    }
    
    public void UpdateScore()
    {
        scoreTMPro.text = $"{playerScores[(int)PlayerColor.Blue]}        {playerScores[(int)PlayerColor.Red]}";
    }

    //[PunRPC]
    //private void RPCUpdateScoreText(string player1ScoreText, string player2ScoreText)
    //{
    //    scoreTMPro.text = $"{player1ScoreText}        {player2ScoreText}";
    //}

    //[PunRPC]
    //private void RPCInitScore()
    //{
    //    scoreTMPro.text = $"0        0";
    //}

    #endregion


    #region ⚙️ MainMenu Panel ⚙️

    #endregion


    #region 🚩 GameResult Panel 🚩

    private void ResultPopUp(float elapse, string winner)
    {
        if (winner == "Draw")
        {
            Debug.Log("Draw");
            if (PhotonNetwork.IsMasterClient)
            {
                GameWinPanel.GetComponent<Image>().sprite = GameResultDraw;
                GameWinPanel.SetActive(true);
                FadeinImige();
                if (elapse >= 5f)
                {
                    GameWinPanel.SetActive(false);
                    BossSpawnNotification.SetActive(true);
                    TextFadeOut();
                    //GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultDraw;
                }
            }
            else
            {
                GameWinPanel.GetComponent<Image>().sprite = GameResultDraw;
                GameWinPanel.SetActive(true);
                FadeinImige();
                if (elapse >= 5f)
                {
                    GameWinPanel.SetActive(false);
                    BossSpawnNotification.SetActive(true);
                    TextFadeOut();
                    //GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultDraw;
                }
            }
        }
        else if (winner == "Blue")
        {
            Debug.Log("Blue");
            if (PhotonNetwork.IsMasterClient)
            {
                GameWinPanel.GetComponent<Image>().sprite = GameWinSprite;
                GameWinPanel.SetActive(true);
                FadeinImige();
                if (elapse >= 5f)
                {
                    GameWinPanel.SetActive(false);
                    GameResultImage.SetActive(true);
                    GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameWinSprite;
                }
            }
            else
            {
                GameWinPanel.GetComponent<Image>().sprite = GameDefeatSprite;
                GameWinPanel.SetActive(true);
                FadeinImige();
                if (elapse >= 5f)
                {
                    GameWinPanel.SetActive(false);
                    GameResultImage.SetActive(true);
                    GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameDefeatSprite;
                }
            }
        }
        else if (winner == "Red")
        {
            Debug.Log("Red");
            if (PhotonNetwork.IsMasterClient)
            {
                GameWinPanel.GetComponent<Image>().sprite = GameDefeatSprite;
                GameWinPanel.SetActive(true);
                FadeinImige();
                if (elapse >= 5f)
                {
                    GameWinPanel.SetActive(false);
                    GameResultImage.SetActive(true);
                    GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameDefeatSprite;
                }
            }
            else
            {
                GameWinPanel.GetComponent<Image>().sprite = GameWinSprite;
                GameWinPanel.SetActive(true);
                FadeinImige();
                if (elapse >= 5f)
                {
                    GameWinPanel.SetActive(false);
                    GameResultImage.SetActive(true);
                    GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameWinSprite;
                }
            }
        }
    }

    // 승패 결과 이미지 팝업 함수
    //private IEnumerator ResultImagePopUp()
    //{
    //    Debug.Log($"result image popup start");
    //    Debug.Log(GameManager.Instance.winner);
    //    if (GameManager.Instance.winner == "Blue")
    //    {
    //        Debug.Log("2 "); 
    //        if (PhotonNetwork.IsMasterClient)
    //        {
    //            GameWinPanel.GetComponent<Image>().sprite = GameWinSprite;
    //            FadeinImige();
    //            Debug.Log("3");
    //            yield return Delay500;
    //            GameWinPanel.SetActive(false);
    //            GameResultImage.SetActive(true);
    //            GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultWin;
    //        }
    //        else
    //        {
    //            GameWinPanel.GetComponent<Image>().sprite = GameDefeatSprite;
    //            FadeinImige();
    //            Debug.Log("4");
    //            yield return Delay500;
    //            GameWinPanel.SetActive(false);
    //            GameResultImage.SetActive(true);
    //            GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultDef;
    //        }
    //    }
    //    else if (GameManager.Instance.winner == "Red")
    //    {
    //        if (PhotonNetwork.IsMasterClient)
    //        {
    //            GameWinPanel.GetComponent<Image>().sprite = GameDefeatSprite;
    //            FadeinImige();

    //            yield return Delay500;
    //            GameWinPanel.SetActive(false);
    //            GameResultImage.SetActive(true);
    //            GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultDef;
    //        }
    //        else
    //        {
    //            GameWinPanel.GetComponent<Image>().sprite = GameWinSprite;
    //            FadeinImige();

    //            yield return Delay500;
    //            GameWinPanel.SetActive(false);
    //            GameResultImage.SetActive(true);
    //            GameResultImage.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameResultWin;
    //        }
    //    }
    //    yield return null;
    //}

    // 페이드 인 함수
    private IEnumerator ImageFadeIn()
    {
        float fadeValue = 0;
        Image gameWinPanel = GameWinPanel.GetComponent<Image>();
        while (fadeValue <= 1.0f)
        {
            fadeValue += 0.01f;
            yield return new WaitForSeconds(0.01f);

            gameWinPanel.color = new Color(255, 255, 255, fadeValue);

            yield return null; // 코루틴 한번 도는지 확인해주는 작업?
        }
    }

    public void ActivationGameWinUI_Nexus(string tag)
    {
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

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
        StartCoroutine(CorPopUp(gameWinMessage));
        GameManager.Instance.isGameEnd = true;
        onGameEnd.Invoke();

        //StartCoroutine(DelayLeaveRoom());
    }

    float nexusTime;
    IEnumerator CorPopUp(string str)
    {
        while (true)
        {
            nexusTime += Time.deltaTime;
            yield return null;
            ResultPopUp(nexusTime, str);
        }
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
        while (true)
        {
            yield return null;
            try
            {
                playerHp = GameManager.Instance.CurrentPlayers[0].GetComponent<Health>();
                Debug.Log(playerHp);
                enemyHp = GameManager.Instance.CurrentPlayers[1].GetComponent<Health>();
                Debug.Log(enemyHp);
                break;
            }
            catch (Exception ie)
            {
                print(ie);
            }
        }
        StopCoroutine(setHp());
    }

    float playerHp2D;
    void UpdateHealthUI()
    {
        if (playerHp == null)
        {
            return;
        }

        playerHealthBar.fillAmount = playerHp.health / playerHp.MaxHealth;
        playerHp2D = playerHp.health;
        playerHealthBarTMpro.text = (int)playerHp2D + " / " + playerHp.MaxHealth;
    }

    IEnumerator SetPlayer()
    {
        while (true)
        {
            try
            {
                PlayerObject = GameManager.Instance.CurrentPlayers[0];
                playerStat = PlayerObject.GetComponent<Stats>();
                playerScript = PlayerObject.GetComponent<Player>();
                yield break;
            }
            catch (Exception)
            {
                Debug.Log("플레이어 가져오는중");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void UpdateStatusUI()
    {
        // 경험치
        playerExperienceBarTMpro.text = $"{playerStat.acquiredExp} / {playerStat.maxExp}";

        // 경험치 바
        playerExperienceBar.fillAmount = playerStat.acquiredExp / playerStat.maxExp;

        // DPS
        PlayerDpsText.text = $"{Math.Round(playerStat.attackDmg / playerStat.attackSpeed)}";

        // 이동속도
        PlayerSpeedText.text = $"{Math.Round(playerStat.moveSpeed)}";

        // 초상화
        PlayerPortrait.sprite = playerScript.playerIcon;

        // 레벨
        PlayerLevelText.text = $"{playerStat.Level}";
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

            if (enemyHp.health <= 0)
            {
                InfoPanel.SetActive(false);
            }

            InfoHealthBar.fillAmount = enemyHp.health / enemyHp.MaxHealth;
            Hp2D = enemyHp.health;
            InfoHealthBarTMPro.text = (int)Hp2D + " / " + enemyHp.MaxHealth;

            float dmg = currentPlayerforInfo.playerStats.attackDmg;
            float atkSpeed = currentPlayerforInfo.playerStats.attackSpeed;
            float moveSpeed = currentPlayerforInfo.playerStats.moveSpeed;
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
            InfoHealthBar.fillAmount = currentTurretforInfo.currentHealth / currentTurretforInfo.maxHealth;
            Hp2D = currentTurretforInfo.currentHealth;
            InfoHealthBarTMPro.text = (int)Hp2D + " / " + currentTurretforInfo.maxHealth;
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
        InfoLevel.text = player.playerStats.Level.ToString();
    }

    public void ActivationTowerInfoUI(Turret turret)
    {
        INFO = InfoState.Tower;
        currentTurretforInfo = turret;
        InfoPanel.SetActive(true);

        float dmg = currentTurretforInfo.attack;
        float atkSpeed = currentTurretforInfo.attackSpeed;
        float dps = dmg * atkSpeed;

        InfoDpsTMpro.text = dps.ToString();
        InfoSpdMpro.text = 0.ToString();
        InfoLevel.text = turret.towerDB.Rank.ToString();
        InfoIcon.sprite = turret.towerDB.Sprite_TowerProtrait;
    }

    public void ActivationMinionInfoUI(Enemybase minion, Sprite icon)
    {
        INFO = InfoState.Minion;
        currentMinionforInfo = minion;
        InfoPanel.SetActive(true);

        float dmg = minion.Damage;
        float atkSpeed = minion.AttackSpeed;
        float spd = minion.moveSpeed;
        float dps = dmg * atkSpeed;

        InfoDpsTMpro.text = dps.ToString();
        InfoSpdMpro.text = spd.ToString();
        InfoLevel.text = minion.minionDB.Rank.ToString();

        // 들어온 아이콘이 용일때 리턴
        if(icon.name == "Dragonknight18")
        {
            InfoIcon.sprite = icon;
            return;
        }

        if (minion.gameObject.tag == "Blue")
        {
            InfoIcon.sprite = minion.minionDB.Icon_Blue;
        }
        else if (minion.gameObject.tag == "Red")
        {
            InfoIcon.sprite = minion.minionDB.Icon_Red;
        }
        else
        {
            InfoIcon.sprite = icon;
        }
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

        while (true)
        {
            fadeValue -= 0.2f;
            yield return new WaitForSeconds(1f);
            if (fadeValue <= 0f && BossMonsterSpawnON == false)
            {
                BossMonsterSpawnON = true;
                GameManager.Instance.bossMonsterSpawn();
                yield break;
            }

            bossText.color = new Color(255, 255, 255, fadeValue);
            yield return null;

        }
    }

    float fadeTest = 1f;
    TextMeshProUGUI bossText;
    public void TextFadeOut()
    {
        fadeTest -= 0.01f;
        if (BossMonsterSpawnON)
        {
            return;
        }

        if (fadeTest <= 0f && BossMonsterSpawnON == false)
        {
            BossMonsterSpawnON = true;
            GameManager.Instance.bossMonsterSpawn();
        }
        bossText.color = new Color(255, 255, 255, fadeTest);
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

    private void OnDisable()
    {
        Instance = null;
    }
}