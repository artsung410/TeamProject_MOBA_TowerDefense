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

    [Header("GameESCUI")]
    public GameObject ESCButton;

    [Header("MousePointer")]
    public Canvas MousePointerCanvas;
    public GameObject MousePositionImage;
    public MousePointer mousePointer;

    private int[] playerScores = { 0, 0 };

    float sec = 0;
    int min = 0;

    // 플레이어 상태창
    private Health playerHp;

    // 인포창 참조자
    private Health enemyHpForInfo;
    private Turret currentTurretForInfo;
    private Enemybase currentMinionForInfo;
    private NexusHp currentNexusForInfo;

    InfoState INFO;

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
    }

    private void Start()
    {
        setSkill();
        StartCoroutine(setHp());
        setMouseCursor();
        photonView.RPC("RPCInitScore", RpcTarget.All);
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
        UpdateInfoHealthUI();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESCButton_S();
        }
    }


    // ################################[ ⋆⁺₊⋆ Skill Panel ⋆⁺₊⋆ ]################################

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


    // ################################[ 🕦 Timer & Scroe Panel 🕦 ]################################


    // 🚩 게임종료 조건1  - 시간 종료시 게임결과창 출력
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

            //photonView.RPC("RPC_ActivationGameWinUI", RpcTarget.All, gameWinMessage);
            min = 0;
            sec = 0;
            timerTMPro.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
            GameManager.Instance.isGameEnd = true;
            onGameEnd.Invoke();

            // 승패 이미지 호출
            StartCoroutine(ResultImagePopUp());

            //StartCoroutine(DelayLeaveRoom());
            return;
        }
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


    // ################################[ 😁 Player Status Panel 😁 ]################################


    // 좌측하단 플레이어 정보 업데이트
    float playerHp2D;
    private IEnumerator setHp()
    {
        yield return new WaitForSeconds(0.5f);
        playerHp = GameManager.Instance.CurrentPlayers[0].GetComponent<Health>();
        enemyHpForInfo = GameManager.Instance.CurrentPlayers[1].GetComponent<Health>();
        StopCoroutine(setHp());
    }

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


    // ################################[ 🕮 Info Panel 🕮 ]################################


    // 좌측 상단 오브젝트 정보 업데이트
    float infoHp2D;

    // 오브젝트 클릭-> 오브젝트의 현재 체력 불러와서 update함수로 돌림 
    public void UpdateInfoHealthUI()
    {
        if (INFO == InfoState.Player)
        {
            if (enemyHpForInfo == null)
            {
                return;
            }

            if (enemyHpForInfo.hpSlider3D.value <= 0)
            {
                InfoPanel.SetActive(false);
            }

            InfoHealthBar.fillAmount = enemyHpForInfo.hpSlider3D.value / enemyHpForInfo.hpSlider3D.maxValue;
            infoHp2D = enemyHpForInfo.hpSlider3D.value;
            InfoHealthBarTMPro.text = infoHp2D + " / " + enemyHpForInfo.hpSlider3D.maxValue;
        }

        else if (INFO == InfoState.Tower)
        {
            if (currentTurretForInfo == null)
            {
                return;
            }

            if (currentTurretForInfo.currentHealth <= 0)
            {
                InfoPanel.SetActive(false);
            }

            InfoHealthBar.fillAmount = currentTurretForInfo.currentHealth / currentTurretForInfo.maxHealth;
            infoHp2D = currentTurretForInfo.currentHealth;
            InfoHealthBarTMPro.text = infoHp2D + " / " + currentTurretForInfo.maxHealth;
        }

        else if (INFO == InfoState.Minion)
        {
            if (currentMinionForInfo == null)
            {
                return;
            }

            if (currentMinionForInfo.CurrnetHP <= 0)
            {
                InfoPanel.SetActive(false);
            }

            InfoHealthBar.fillAmount = currentMinionForInfo.CurrnetHP / currentMinionForInfo.HP;
            infoHp2D = currentMinionForInfo.CurrnetHP;
            InfoHealthBarTMPro.text = infoHp2D + " / " + currentMinionForInfo.HP;
        }

        else if (INFO == InfoState.Nexus)
        {
            if (currentNexusForInfo == null)
            {
                return;
            }

            if (currentNexusForInfo.CurrentHp <= 0)
            {
                InfoPanel.SetActive(false);
            }

            InfoHealthBar.fillAmount = currentNexusForInfo.CurrentHp / currentNexusForInfo.MaxHp;
            infoHp2D = currentNexusForInfo.CurrentHp;
            InfoHealthBarTMPro.text = infoHp2D + " / " + currentNexusForInfo.MaxHp;
        }


    }

    // 상대 챔프 정보 출력
    public void ActivationEnemyInfoUI(Stats st, Sprite icon)
    {
        INFO = InfoState.Player;
        InfoPanel.SetActive(true);

        InfoIcon.sprite = icon;
        float dmg = st.attackDmg;
        float atkSpeed = st.attackSpeed;
        float spd = st.MoveSpeed;

        float dps = dmg * atkSpeed;
        InfoDpsTMpro.text = dps.ToString();
        InfoSpdMpro.text = spd.ToString();
    }

    // 타워 정보 출력
    public void ActivationTowerInfoUI(Turret turret, Item item, string tag)
    {
        INFO = InfoState.Tower;
        currentTurretForInfo = turret;
        InfoPanel.SetActive(true);

        // 이벤트로 들어온 매개변수 세팅(Item class)
        InfoIcon.sprite = item.itemIcon;
        float dmg = turret.damage;
        float atkSpeed = turret.fireRate;

        float dps = dmg * atkSpeed;
        InfoDpsTMpro.text = dps.ToString();
        InfoSpdMpro.text = 0.ToString();
    }

    // 미니언 정보 출력
    public void ActivationMinionInfoUI(Enemybase minion, Sprite icon)
    {
        INFO = InfoState.Minion;
        currentMinionForInfo = minion;
        InfoPanel.SetActive(true);

        InfoIcon.sprite = icon;
        float dmg = minion.Damage;
        float atkSpeed = minion.AttackSpeed;
        float spd = minion.moveSpeed;

        float dps = dmg * atkSpeed;
        InfoDpsTMpro.text = dps.ToString();
        InfoSpdMpro.text = spd.ToString();
    }

    // 넥서스 정보 출력
    public void ActivationNexusInfoUI(NexusHp nexus, Sprite icon)
    {
        INFO = InfoState.Nexus;
        currentNexusForInfo = nexus;
        InfoPanel.SetActive(true);

        InfoIcon.sprite = icon;
        InfoDpsTMpro.text = 0.ToString();
        InfoSpdMpro.text = 0.ToString();
    }


    // ################################[ ⚙️ MainMenu Panel ⚙️ ]################################


    // ESC 버튼을 누르면 창이 켜진다.
    private void ESCButton_S()
    {
        ESCButton.SetActive(true);
    }


    // ################################[ 🚩 GameResult Panel 🚩 ]################################


    // 승패 결과 이미지 팝업 함수
    private IEnumerator ResultImagePopUp()
    {
        // 오브젝트 활성화
        GameWinPanel.SetActive(true);

        //GameManager.Instance.winner = "Blue";

        // 승자가 블루면
        if (GameManager.Instance.winner == "Blue")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameWinPanel.GetComponent<Image>().sprite = GameWinSprite;
                StartCoroutine(ImageFadeIn());

                yield return new WaitForSeconds(5f);
                GameWinPanel.SetActive(false);
                GameResultImage.SetActive(true);
                GameResultImage.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameResultWin;
            }
            else
            {
                GameWinPanel.GetComponent<Image>().sprite = GameDefeatSprite;
                StartCoroutine(ImageFadeIn());

                yield return new WaitForSeconds(5f);
                GameWinPanel.SetActive(false);
                GameResultImage.SetActive(true);
                GameResultImage.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameResultDef;
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameWinPanel.GetComponent<Image>().sprite = GameDefeatSprite;
                StartCoroutine(ImageFadeIn());

                yield return new WaitForSeconds(5f);
                GameWinPanel.SetActive(false);
                GameResultImage.SetActive(true);
                GameResultImage.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameResultDef;
            }
            else
            {
                GameWinPanel.GetComponent<Image>().sprite = GameWinSprite;
                StartCoroutine(ImageFadeIn());

                yield return new WaitForSeconds(5f);
                GameWinPanel.SetActive(false);
                GameResultImage.SetActive(true);
                GameResultImage.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameResultWin;
            }
        }
    }

    // 페이드 인 함수
    private IEnumerator ImageFadeIn()
    {
        float fadeValue = 0;

        Image gameWinPanel = GameWinPanel.GetComponent<Image>();

        while (fadeValue < 1.0f)
        {
            fadeValue += 0.01f;

            yield return new WaitForSeconds(0.01f);

            gameWinPanel.color = new Color(255, 255, 255, fadeValue);
        }
    }

    // 🚩 게임종료 조건2 - 넥서스 파괴시 게임결과창 출력
    public void ActivationGameWinUI_Nexus(string tag)
    {
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

        GameManager.Instance.isGameEnd = true;
        onGameEnd.Invoke();

        string gameWinMessage = "";

        GameManager.Instance.winner = "Blue";

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


    // ################################[ 🔑 InGame System 🔑 ]################################

    // 로비 탈출
    private IEnumerator DelayLeaveRoom()
    {
        yield return new WaitForSeconds(7f);
        PhotonNetwork.LeaveRoom();
    }

    // 마우스 커서 세팅 (이동, 어택땅)
    private void setMouseCursor()
    {
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMouseCanvas = MousePointerCanvas;
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMouseObj = MousePositionImage;
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMousePointer = mousePointer;
    }
}
