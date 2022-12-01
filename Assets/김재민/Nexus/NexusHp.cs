using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;
public class NexusHp : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    public static event Action<NexusHp, Sprite> nexusMouseDownEvent = delegate { };

    public float CurrentHp;
    public float MaxHp;
    private bool isDie;
    public GameObject destroyParticle;

    [Header("Hp바")]
    public Sprite[] healthbarImages = new Sprite[3];  // 적군 아군 체력바 구별
    public Image healthbarImage; // hp바 
    public Image hitbarImage; // hit바
    public GameObject ui; // Hp바 캔버스


    public Sprite nexusSprite;

    [SerializeField]
    GameObject healthEffect;
    WaitForSeconds Dealay100 = new WaitForSeconds(1);

    Outline _outline;
    string myTag;
    private void Awake()
    {
        _outline = GetComponent<Outline>();
        CurrentHp = MaxHp;
        _outline.enabled = false;
        _outline.OutlineWidth = 8f;
        healthEffect.SetActive(false);

    }


    private void OnEnable()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                gameObject.tag = "Blue";

            }
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                gameObject.tag = "Red";
            }
        }
        if (gameObject == null)
        {
            return;
        }

        if (photonView.IsMine)
        {
            healthbarImage.sprite = healthbarImages[0]; // 초록 
            hitbarImage.sprite = healthbarImages[1]; //빨강
        }
        else
        {
            healthbarImage.sprite = healthbarImages[1]; // 빨강
            hitbarImage.sprite = healthbarImages[2]; // 노랑
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(RegenerationSwitch), 0, 1f);

    }


    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }


        if (transform == null || transform.gameObject == null || this == null)
        {
            return;
        }

    }

    public void TakeOnDagmage(float Damage)
    {
        if (PlayerHUD.Instance.BossMonsterSpawnON == false)
        {
            photonView.RPC("RPC_TakeDamage", RpcTarget.All, Damage);

        }
    }

    [PunRPC]
    public void RPC_TakeDamage(float Damage)
    {
        if (isDie == true)
        {
            return;
        }

        if (isDie == false)
        {

            CurrentHp -= Damage;
            CurrentHp = Mathf.Max(CurrentHp - Damage, 0);
            healthbarImage.fillAmount = CurrentHp / MaxHp;
            StartCoroutine(ApplyHitBar(healthbarImage.fillAmount));
            if (CurrentHp <= 0)
            {
                isDie = true;
                PlayerHUD.Instance.ActivationGameWinUI_Nexus(gameObject.tag);
                PhotonNetwork.Instantiate(destroyParticle.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);

            }
        }
    }

    private IEnumerator ApplyHitBar(float value)
    {
        float prevValue = hitbarImage.fillAmount;
        float delta = prevValue / 100f;

        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            prevValue -= delta;
            hitbarImage.fillAmount = prevValue;

            if (prevValue - value < 0.001f)
            {
                break;
            }
        }
    }


    private void OnMouseDown()
    {
        nexusMouseDownEvent.Invoke(this, nexusSprite);
    }

    private void RegenerationSwitch()
    {
        Collider[] WeTeam = Physics.OverlapSphere(gameObject.transform.position, 15);

        foreach (Collider col in WeTeam)
        {
            if (col.gameObject.layer == 12 && col.CompareTag(gameObject.tag)) // 자기 자신 제외
            {
                continue;
            }

            if (col.CompareTag(gameObject.tag) && col.gameObject.layer == 7 && photonView.IsMine) //우리팀이랑 태그 같고 플레이어 + 자기자신에게만 
            {
                if (col.GetComponent<Health>().health < col.GetComponent<Stats>().maxHealth)
                {
                    photonView.RPC("effectSwich", RpcTarget.All, true);
                    col.GetComponent<Health>().Regenation(25f);

                }
                else
                {
                    photonView.RPC("effectSwich", RpcTarget.All, false);

                }
            }


        }
    }

    [PunRPC]
    private void effectSwich(bool value)
    {
        healthEffect.SetActive(value);
    }

    private void OnMouseEnter()
    {

        //if(PlayerHUD.Instance.cursorMoveEnemy == null || PlayerHUD.Instance.cursorMoveAlly == null)
        //{
        //    return;
        //}

        if (photonView.IsMine) // 자기 자신이면 켜주고  색 그린
        {
            //Cursor.SetCursor(PlayerHUD.Instance.cursorMoveAlly, Vector2.zero, CursorMode.Auto);
            _outline.OutlineColor = Color.green;
            _outline.enabled = true; // 켜주고
        }
        else
        {
            //Cursor.SetCursor(PlayerHUD.Instance.cursorMoveEnemy, Vector2.zero, CursorMode.Auto);
            _outline.OutlineColor = Color.red;
            _outline.enabled = true;
        }

    }

    private void OnMouseExit()
    {
        //Cursor.SetCursor(PlayerHUD.Instance.cursorMoveNamal, Vector2.zero, CursorMode.Auto);
        _outline.enabled = false;
    }



}
