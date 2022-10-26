using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerHUD : MonoBehaviourPun
{
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

    private Health playerHp;
    private Health enemyHp;

    [Header("MousePointer")]
    public Canvas MousePointerCanvas;
    public GameObject MousePositionImage;
    public MousePointer mousePointer;

    private int[] playerScores = { 0, 0 };

    private void Awake()
    {
        Instance = this;
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
        GameManager.Instance.CurrentPlayers [0].GetComponent<PlayerBehaviour>().moveMouseCanvas = MousePointerCanvas;
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMouseObj = MousePositionImage;
        GameManager.Instance.CurrentPlayers[0].GetComponent<PlayerBehaviour>().moveMousePointer = mousePointer;
    }

    float sec;
    int min;

    void Update()
    {
        Timer();
        UpdateHealthUI();
        UpdateEnemyHealthUI();
    }

    void Timer()
    {
        sec += Time.deltaTime;
        timerTMPro.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);

        if ((int)sec > 59)
        {
            sec = 0;
            min++;
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
            playerScores[0] += 1;
        }
        else
        {
            playerScores[1] += 1;
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
}
