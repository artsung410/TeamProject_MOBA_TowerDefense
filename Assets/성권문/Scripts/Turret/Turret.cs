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
    public static event Action<GameObject, float> OnTurretDestroyEvent = delegate { };

    [Header("인게임 DB")]
    public TowerData towerData;

    private Outline _outline;


    [HideInInspector]
    public float currentHealth; // 현재체력

    [Header("Hp바")]
    public Image healthbarImage; // hp바 
    public Image hitbarImage; // hit바
    public GameObject ui; // Hp바 캔버스
    private GameObject newDestroyParticle; // 타워 파괴효과를 담을 변수

    [HideInInspector]
    public float attack; // 공격력

    [HideInInspector]
    public float attackSpeed; // 공격속도

    [Header("회전체")]
    public Transform partToRotate;

    [Header("투사체 발사 위치")]
    public Transform firePoint;

    // 타워헤드 회전속도
    protected float turnSpeed = 10f;

    // 타겟
    protected string enemyTag;
    protected Transform target;
    protected EnemyMinion targetEnemy;
    protected Transform shotTransform;
    protected float fireCountdown = 0f;

    // 공격범위 표시
    [Header("공격 도형")]
    public GameObject DangerZonePF;
    protected GameObject NewDangerZone;

    // 타워 효과음
    protected private AudioSource audioSource;

    void Awake()
    {
        // 타워 데이터 -> 타워의 체력 적용
        currentHealth = towerData.MaxHP;

        // 타워 데이터 -> 투사체의 공격력 적용
        attack = towerData.Attack;

        // 타워 데이터 -> 타워의 공격 주기 적용
        attackSpeed = towerData.AttackSpeed;

        _outline = GetComponent<Outline>();

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
            Debug.Log("마스터 클라이언트 태그 적용");
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
            Debug.Log("일반 클라이언트 태그 적용");
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

        // [自 -> Event] 미니언PF가 존재하면 MinionSpawner에게 알리기. 
        if (towerData.ObjectPF != null)
        {
            minionTowerEvent.Invoke(towerData.ObjectPF, gameObject.tag);
        }

        _outline.enabled = false;
        _outline.OutlineWidth = 8f;
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

    float exp = 100f;
    [PunRPC]
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        healthbarImage.fillAmount = currentHealth / towerData.MaxHP;
        StartCoroutine(ApplyHitBar(healthbarImage.fillAmount));
        if (currentHealth <= 0)
        {
            // 타워 파괴시 경험치
            OnTurretDestroyEvent.Invoke(gameObject, exp);

            photonView.RPC("Destroy", RpcTarget.All);
            return;
        }
    }

    // red_hitbar lerp 수동으로 적용
    private IEnumerator ApplyHitBar(float value)
    {
        float prevValue = hitbarImage.fillAmount;
        float delta = prevValue / 100f;

        while(true)
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

    // =========================== 타워 파괴 처리 ===========================
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
        Destroy(gameObject);
    }

    // =========================== 타워 버프 적용 처리 ===========================
    public void incrementBuffValue(int id, float addValue, bool state)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        //photonView.RPC("RPC_ApplyTowerBuff", RpcTarget.All, id, addValue, state);
    }

    //[PunRPC]
    //public void RPC_ApplyBuff(int id, float value, bool st)
    //{
    //    if (id == (int)Buff_Effect_byTower.AtkUP)
    //    {
    //        if (st)
    //        {
    //            attack += value;
    //            towerData.Projectiles.GetComponent<Projectiles>().damage += value;

    //        }
    //        else
    //        {
    //            attack -= value;
    //            towerData.Projectiles.GetComponent<Projectiles>().damage -= value;
    //        }
    //    }

    //    else if (id == (int)Buff_Effect_byTower.AtkSpeedUp)
    //    {
    //        if (st)
    //        {
    //            attackSpeed += value;
    //        }
    //        else
    //        {
    //            attackSpeed -= value;
    //        }
    //    }
    //}

    // =========================== 타겟 추적 ===========================

    // 가장 가까운 타겟 찾기.
    protected void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // 가장 가까운 적과의 거리
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // 적이 범위안에 들어왔고, 적과의 거리가 범위값보다 작을경우
        if (nearestEnemy != null && shortestDistance <= towerData.AttackRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    // 타겟방향으로 회전하기.
    protected void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    // =========================== 스킬타워 공격처리 ===========================
    protected virtual void Fire()
    {
        // 스킬 정의.
    }

    // =========================== 물리타워 공격처리 ===========================
    protected virtual void Shoot()
    {
        // 투사체 정의.
    }

    // =========================== 타워 툴팁 적용 ===========================
    private void OnMouseDown()
    {
        turretMouseDownEvent.Invoke(this);
    }

    // =========================== 타워 아웃라인 적용 ===========================
    private void OnMouseEnter()
    {
        if (photonView.IsMine) // 자기 자신이면 켜주고  색 그린
        {

            _outline.OutlineColor = Color.green;
            _outline.enabled = true; // 켜주고
        }
        else
        {

            _outline.OutlineColor = Color.red;
            _outline.enabled = true;
        }
    }
    private void OnMouseExit()
    {
        _outline.enabled = false;
    }

}
