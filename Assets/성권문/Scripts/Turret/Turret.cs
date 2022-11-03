using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Turret : MonoBehaviourPun
{
    public static event Action<Turret, Item, string> turretMouseDownEvent = delegate { };

    [Header("인게임 DB")]
    [SerializeField]
    public TowerData towerData;

    public float currentHealth;
    public Image healthbarImage;
    public GameObject ui;
    public GameObject destroyParticle;
    private GameObject newDestroyParticle;
    public float destorySpeed;

    [Header("타워카드 DB")]
    public Item towerItem;

    [HideInInspector]
    public string enemyTag;

    [HideInInspector]
    public float fireCountdown = 0f;

    protected void Awake()
    {
        if (towerData.ObjectPF.layer == 14)
        {
            towerData.ObjectPF.GetComponent<Projectiles>().damage = towerData.Attack;
        }

        PlayerHUD.onGameEnd += Destroy_gameEnd;
    }

    protected void OnEnable()
    {
        currentHealth = towerData.MaxHP;
        GameManager.Instance.CurrentTurrets.Add(gameObject);

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                gameObject.tag = "Blue";
                enemyTag = "Red";
            }

            else
            {
                gameObject.tag = "Red";
                enemyTag = "Blue";
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                gameObject.tag = "Red";
                enemyTag = "Blue";
            }

            else
            {
                gameObject.tag = "Blue";
                enemyTag = "Red";
            }
        }
    }

    public void Damage(float damage)
    {
        //Debug.Log("Damage 적용");

        // 게임 끝나면 정지
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

        photonView.RPC("TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        //Debug.Log("Damage RPC적용");

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        healthbarImage.fillAmount = currentHealth / towerData.MaxHP;

        if (currentHealth <= 0)
        {
            Debug.Log("데미지 들어감!!!!");
            photonView.RPC("Destroy", RpcTarget.All);
            return;
        }
    }

    public void Destroy_gameEnd()
    {
        if (this == null)
        {
            return;
        }

        photonView.RPC("Destroy", RpcTarget.All);
    }

    [PunRPC]
    public void Destroy()
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }

        StartCoroutine(Destructing());
        newDestroyParticle = PhotonNetwork.Instantiate(destroyParticle.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
        StartCoroutine(Destruction(newDestroyParticle));
    }

    IEnumerator Destructing()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(Vector3.down * Time.deltaTime * destorySpeed);

            if (transform.position.y < -10)
            {
                StopCoroutine(Destructing());

                if (newDestroyParticle != null)
                {
                    StopCoroutine(Destructing());
                    StopCoroutine(Destruction(newDestroyParticle));
                }
            }
        }
    }

    IEnumerator Destruction(GameObject particle)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(particle);
        gameObject.SetActive(false);
    }

    public void additionalAtk(float addValue)
    {
        towerData.additional_Atk += addValue;
    }

    public void additionalAtkSpd(float addValue)
    {
        towerData.additional_AtkSpd += addValue;
    }

    // 타워 클릭했을 때 툴팁뜨게하기
    private void OnMouseDown()
    {
        turretMouseDownEvent.Invoke(this, towerItem, gameObject.tag);
    }
}
