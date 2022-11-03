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
    public static event Action<Turret> turretMouseDownEvent = delegate { };

    [Header("인게임 DB")]
    [SerializeField]
    public TowerData towerData;
    public float currentHealth;
    public Image healthbarImage;
    public GameObject ui;
    public GameObject destroyParticle;
    private GameObject newDestroyParticle;
    public float destorySpeed;

    [HideInInspector]
    public string enemyTag;

    [HideInInspector]
    public float fireCountdown = 0f;

    public float attack;
    public float attackSpeed;

    void Awake()
    {
        attack = towerData.Attack;
        attackSpeed = towerData.AttackSpeed;

        if (towerData.ObjectPF.layer == 14)
        {
            towerData.ObjectPF.GetComponent<Projectiles>().damage = attack;
        }

        BuffManager.towerBuffAdditionEvent += incrementBuffValue;
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

    // 타워 버프효과 발동
    public void incrementBuffValue(int id, float addValue, bool state)
    {
        photonView.RPC("RPC_ApplyTowerBuff", RpcTarget.All, id, addValue, state);
    }

    [PunRPC]
    public void RPC_ApplyTowerBuff(int id, float value, bool st)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (id == (int)Buff_Effect.AtkUP)
        {
            if (st)
            {
                Debug.Log("타워 공격력 증가!");
                attack += value;

                if (towerData.ObjectPF.layer == 14)
                {
                    towerData.ObjectPF.GetComponent<Projectiles>().damage += value;
                }
            }
            else
            {
                Debug.Log("타워 공격력 증가 종료!");
                attack -= value;

                if (towerData.ObjectPF.layer == 14)
                {
                    towerData.ObjectPF.GetComponent<Projectiles>().damage -= value;
                }
            }
        }

        else if (id == (int)Buff_Effect.AtkSpeedUp)
        {
            if (st)
            {
                Debug.Log("타워 공속 증가!");
                attackSpeed += value;
            }
            else
            {
                Debug.Log("타워 공속 증가 종료!");
                attackSpeed -= value;
            }
        }
    }

    // 타워 클릭했을 때 툴팁뜨게하기
    private void OnMouseDown()
    {
        turretMouseDownEvent.Invoke(this);
    }
}
