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
    public static event Action<GameObject,string> minionTowerEvent = delegate { };
    public static event Action<Turret> turretMouseDownEvent = delegate { };

    [Header("인게임 DB")]
    public TowerData towerData;

    [HideInInspector]
    public float currentHealth; // 현재체력

    [Header("Hp바")]
    public Image healthbarImage; // hp바 
    public GameObject ui; // Hp바 캔버스

    private GameObject newDestroyParticle; // 타워 파괴효과를 담을 변수

    [HideInInspector]
    public string enemyTag;

    [HideInInspector]
    public float fireCountdown = 0f;

    [HideInInspector]
    public float attack; // 공격력

    [HideInInspector]
    public float attackSpeed; // 공격속도

    [Header("회전체")]
    public Transform partToRotate;

    [Header("회전속도")]
    public float turnSpeed = 10f;

    void Awake()
    {
        // 타워 데이터 -> 타워의 체력 적용
        currentHealth = towerData.MaxHP;

        // 타워 데이터 -> 투사체의 공격력 적용
        attack = towerData.Attack;

        // 타워 데이터 -> 타워의 공격 주기 적용
        attackSpeed = towerData.AttackSpeed;

        // 투사체의 공격력 처리
        if (towerData.Projectiles != null)
        {
            towerData.Projectiles.GetComponent<Projectiles>().damage = attack;
        }

        //// [自 -> Event] 미니언PF가 존재하면 MinionSpawner에게 알리기. 
        //if (towerData.ObjectPF != null)
        //{
        //    minionTowerEvent.Invoke(towerData.ObjectPF, gameObject.tag);
        //}

        // [Event -> 自] 타워가 버프를 적용받을수 있도록 세팅 
        BuffManager.towerBuffAdditionEvent += incrementBuffValue;

        // [Event -> 自] 게임이 끝나면 타워가 파괴할수 있도록 세팅
        PlayerHUD.onGameEnd += Destroy_gameEnd;
    }

    protected void OnEnable()
    {
        // 게임매니저 상에 타워리스트 등록
        GameManager.Instance.CurrentTurrets.Add(gameObject);

        // 피아식별
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

    // =========================== 타워 데미지 처리 ===========================
    public void Damage(float damage)
    {
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

    // =========================== 타워 데미지 파괴 처리 ===========================
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
        newDestroyParticle = PhotonNetwork.Instantiate(towerData.DestroyPF.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
        StartCoroutine(Destruction(newDestroyParticle));
    }

    IEnumerator Destructing()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(Vector3.down * Time.deltaTime * towerData.DestroySpeed);

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

    // =========================== 타워 버프 적용 처리 ===========================
    public void incrementBuffValue(int id, float addValue, bool state)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("RPC_ApplyTowerBuff", RpcTarget.All, id, addValue, state);
    }

    [PunRPC]
    public void RPC_ApplyTowerBuff(int id, float value, bool st)
    {
        if (id == (int)Buff_Effect.AtkUP)
        {
            if (st)
            {
                Debug.Log("타워 공격력 증가!");
                attack += value;
                towerData.Projectiles.GetComponent<Projectiles>().damage += value;

            }
            else
            {
                Debug.Log("타워 공격력 증가 종료!");
                attack -= value;
                towerData.Projectiles.GetComponent<Projectiles>().damage -= value;
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

    // =========================== 타워 툴팁 적용 ===========================
    private void OnMouseDown()
    {
        turretMouseDownEvent.Invoke(this);
    }
}
