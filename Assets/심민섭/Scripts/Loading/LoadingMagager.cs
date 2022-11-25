using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

// �κ񿡼� �ε����� �Ѿ ��, �߻��� �ϵ��� �����Ѵ�.
// 1. �ε� 0% ~ 100% - � �������� �� ���� ���ؾ��Ѵ�.
// 2. 100%�� �Ǹ� Prototype_1 ������ ��� �ѱ��.

public enum LoadingCharactorType
{
    Warrior = 0,
    Wizard = 1,
}

public class LoadingMagager : MonoBehaviourPunCallbacks
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################


    // isMain �׽�Ʈ
    // Player ���� ������
    public GameObject playerPrefab;
    // ���� �ʻ�ȭ
    public GameObject playerSmallPrefab;
    // VS �̹���
    public GameObject VSImage;

    // �ε� ��Ȳ�� ������ �ؽ�Ʈ
    [SerializeField]
    private Text loadingText;

    // �ε� ������ ����
    private float loadingGage = 0f;

    // ��Ÿ Ÿ�� ����
    private float deltaTime;

    // ��޸� �ð�
    private float waitTime = 30f;

    public static LoadingMagager Instance;


    private void Awake()
    {
        Instance = this;
        PhotonNetwork.AutomaticallySyncScene = true;
        loadingText.text = 0.ToString();
    }


    public List<Sprite> heroImages = new List<Sprite>();
    public List<Transform> loadingCharactorPos = new List<Transform>();
    public List<Sprite> heroImages_small = new List<Sprite>();
    public List<Transform> loadingCharactorSmallPos = new List<Transform>();
    public Transform VSPos;

    private void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate(playerSmallPrefab.name, Vector3.zero, Quaternion.identity);

        PhotonNetwork.Instantiate(VSImage.name, VSPos.position, Quaternion.identity);
    }
    // �켱 �ӽ÷� �ð��� �´� �ε� �������� �÷��ش�.
    private void Update()
    {
        // ���� �ִ� ���� �ε����̸�
        if (SceneManager.GetActiveScene().name == "Loading")
        {
            deltaTime += Time.deltaTime * waitTime;
            loadingGage = (int)deltaTime;
            if (loadingGage == 101)
            {
                loadingGage = 100;
                ChangeMainScene();
            }
            loadingText.text = loadingGage.ToString() + "%";
        }
    }

    // �÷��̾� ��ġ
    // ���̵��
    // 1. �÷��̾� Ȧ�̸� �Ʒ�, ¦�̸� �� ����
    // 2. ��Ʈ��ũ �󿡼� ���� �� ������ �� �ִ� ���� �ʿ���
    // 3. isMain�� ����Ѵٰ� �ϸ� �� �Է��� �����Ҷ�, �Ʒ� �Է��� �Ұ��� �� �÷��̾�� ��

    public void ChangeMainScene()
    {
        //Debug.Log(PhotonNetwork.IsConnected);

        // ������ ������ ������ �Ǿ��ְ� �ε� �������� 100%��
        if (PhotonNetwork.IsConnected && loadingGage == 100)
        {
            SceneManager.LoadScene("demoSmaller");
        }
    }
}
