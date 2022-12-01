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
    public static event Action<GameObject, float> OnTurretDestroyEvent = delegate { };


    [Header("타워 DB (디버그용, 추후에 Private로 전환예정")]
    public TowerBlueprint towerDB;
    private Outline _outline;

    [Header("Hp바")]
    public Sprite[] healthbarImages = new Sprite[3];
    public Image healthbarImage; // hp바 
    public Image hitbarImage; // hit바
    public GameObject ui; // Hp바 캔버스
    private GameObject newDestroyParticle; // 타워 파괴효과를 담을 변수

    [Header("타워 이펙트")]
    public GameObject effectParticles;
    private GameObject newEffectParticles;

    [HideInInspector]
    public float maxHealth; // 초기체력

    [HideInInspector]
    public float currentHealth; // 현재체력

    [HideInInspector]
    public float attack; // 공격력

    [HideInInspector]
    public float attackSpeed; // 공격속도

    protected float range; // 공격범위

    protected GameObject destroyPF; // 타워파괴 파티클
    protected float destroySpeed = 10f; // 타워 파괴 속도

    protected GameObject projectilePF; // 투사체 프리팹
    protected float projectiles_Speed; // 투사체 속도

    [Header("바디")]
    public GameObject fowardBody;

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
        // 오디오 할당
        audioSource = GetComponent<AudioSource>();

        // 타워 아웃라인 컴포넌트 할당
        _outline = GetComponent<Outline>();

        // [Event -> 自] 게임이 끝나면 타워가 파괴할수 있도록 세팅
        PlayerHUD.onGameEnd += Destroy_gameEnd;
    }

    public void SetInitData(int id)
    {
        photonView.RPC(nameof(RPC_SetInitData), RpcTarget.All, id);

        if (photonView.IsMine)
        {
            // 타워 데이터 -> 버프타워 찾아서 버프적용
            if (towerDB.Type == (int)Tower_Type.Buff_Tower || towerDB.Type == (int)Tower_Type.DeBuff_Tower)
            {
                Debug.Log("★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★");
                StartCoroutine(SetBuff());
            }
        }
    }

    [PunRPC]
    public void RPC_SetInitData(int id)
    {
        towerDB = CSVtest.Instance.TowerDic[id];

        currentHealth = maxHealth = towerDB.Hp;

        // 타워 데이터 -> 투사체의 공격력 적용
        attack = towerDB.Attack;

        // 타워 데이터 -> 타워의 공격 주기 적용
        attackSpeed = towerDB.Attack_Speed;

        // 타워 데이터 -> 타워의 공격 범위 적용
        range = towerDB.Range;

        // 타워 데이터 -> 타워의 파괴 프리팹 적용
        destroyPF = towerDB.Destroy_Effect_Pf;

        // 타워 데이터 -> 타워의 투사체 적용
        projectilePF = towerDB.Projectile_Pf;

        // 타워 데이터 -> 타워 투사체 속도적용
        projectiles_Speed = towerDB.Projectile_Speed;

        //Debug.Log($"[타워]{ PhotonNetwork.LocalPlayer.ActorNumber}월드 {gameObject.name} 초기데이터 세팅 완료");
    }

    private IEnumerator SetBuff()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        BuffBlueprint buff = CSVtest.Instance.BuffDic[towerDB.buffID];
        Debug.Log(buff.Name);
        BuffManager.Instance.AddBuff(buff);
    }

    protected void OnEnable()
    {
        // 게임매니저 상에 타워리스트 등록
        GameManager.Instance.CurrentTurrets.Add(gameObject);

        // 피아식별
        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("마스터 클라이언트 태그 적용");
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

        if (!photonView.IsMine)
        {
            hitbarImage.sprite = healthbarImages[2];
            healthbarImage.sprite = healthbarImages[1];
        }
        //else
        //{
        //    hitbarImage.sprite = healthbarImages[1];
        //    healthbarImage.sprite = healthbarImages[0];
        //}

        _outline.enabled = false;
        _outline.OutlineWidth = 8f;
    }

    // =========================== 타워 데미지 처리 / 파괴처리 ===========================
    public void Damage(float damage)
    {
        // 게임 끝나면 정지
        if (GameManager.Instance.isGameEnd == true)
        {
            return;
        }

        photonView.RPC(nameof(TakeDamage), RpcTarget.All, damage);
    }

    float exp = 100f;
    [PunRPC]
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0 )
        {
            return;
        }

        currentHealth -= damage;
        healthbarImage.fillAmount = currentHealth / maxHealth;
        StartCoroutine(ApplyHitBar(healthbarImage.fillAmount));
        if (currentHealth <= 0)
        {
            // 타워 파괴시 경험치
            OnTurretDestroyEvent.Invoke(gameObject, exp);
            if (photonView.IsMine)
            {
                newDestroyParticle = PhotonNetwork.Instantiate(destroyPF.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
                AudioSource explosionAudiosource = newDestroyParticle.GetComponent<AudioSource>();
                explosionAudiosource.clip = towerDB.AudioClip_Destroy;
                explosionAudiosource.Play();

                StartCoroutine(Destruction(newDestroyParticle));

                if (newEffectParticles != null)
                {
                    PhotonNetwork.Destroy(newEffectParticles);
                }

                PhotonNetwork.Destroy(gameObject);
            }
            return;
        }
    }

    // red_hitbar lerp 수동으로 적용
    private IEnumerator ApplyHitBar(float value)
    {
        float prevValue = hitbarImage.fillAmount;
        float delta = prevValue / 100f;

        while (true)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.01f);
            prevValue -= delta;
            hitbarImage.fillAmount = prevValue;

            if (prevValue - value < 0.001f)
            {
                hitbarImage.fillAmount = healthbarImage.fillAmount;
                break;
            }
        }
    }

    // =========================== 외부(게임결과) -> 타워 파괴 처리 ===========================
    public void Destroy_gameEnd()
    {
        if (this == null)
        {
            return;
        }

        if (!photonView.IsMine)
        {
            return;
        }

        PhotonNetwork.Destroy(gameObject);

        if (newEffectParticles != null)
        {
            PhotonNetwork.Destroy(newEffectParticles);
        }
    }

    [PunRPC]
    public void Destroy()
    {
        //if (gameObject.activeSelf == false)
        //{
        //    return;
        //}

        //StartCoroutine(Destructing());
        //newDestroyParticle = PhotonNetwork.Instantiate(destroyPF.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
        //StartCoroutine(Destruction(newDestroyParticle));
    }

    IEnumerator Destruction(GameObject particle)
    {
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        PhotonNetwork.Destroy(particle);
    }

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
        if (nearestEnemy != null && shortestDistance <= range)
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
    // =========================== 타워 이펙트 생성 ===========================

    protected void InitEffectParticles(Vector3 initPos)
    {
        newEffectParticles = PhotonNetwork.Instantiate(effectParticles.name, initPos, Quaternion.identity);
        newEffectParticles.transform.Rotate(new Vector3(0, -90f, 0));
    }

    // =========================== 스킬타워 공격처리 ===========================
    //TODO: [★Artsung] 미니언 우선타격 적용 -> 미니언 없을 때만 플레이어 공격하도록 설정
    //TODO: [★Artsung] 적 공격시 플레이어 자신이 우선타격 하도록 설정
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
            Cursor.SetCursor(PlayerHUD.Instance.cursorMoveAlly, Vector2.zero, CursorMode.Auto);
            _outline.OutlineColor = Color.green;
            _outline.enabled = true; // 켜주고
        }
        else
        {
            Cursor.SetCursor(PlayerHUD.Instance.cursorMoveEnemy, Vector2.zero, CursorMode.Auto);
            _outline.OutlineColor = Color.red;
            _outline.enabled = true;
        }
    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(PlayerHUD.Instance.cursorMoveNamal, Vector2.zero, CursorMode.Auto);
        _outline.enabled = false;
    }

}
