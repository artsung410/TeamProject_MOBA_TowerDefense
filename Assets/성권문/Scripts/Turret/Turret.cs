using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;

    [Header("사거리")]
    public float range = 15f;

    [Header("공격주기(초당 n번 발사)")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("타겟 TAG")]
    public string enemyTag = "Enemy";

    [Header("회전체")]
    public Transform partToRotate;

    [Header("회전속도")]
    public float turnSpeed = 10f;

    [Header("공격범위 도형")]
    public GameObject dangerZonePrefab;
    private GameObject dangerZone;
    Vector3 dangerZonePrevPos;
    private bool isEnemyInRange;

    [Header("====== 투사체 ======")]

    [Header("투사체 프리팹")]
    public GameObject bulletPrefab;

    [Header("투사체 발사 위치")]
    public Transform firePoint;

    [Header("====== 단일 레이저 속성======")]
    public bool useLaser = false;
    public int damageOverTime = 30;
    public float slowAmount = 0.5f;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    private void Start()
    {

        // 미사일 터렛일때만 dangerZone가져오기 
        if (gameObject.tag == "CannonTower")
        {
            dangerZone = Instantiate(dangerZonePrefab, transform.position, transform.rotation);
            dangerZone.SetActive(false);
        }

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // 가장 가까운 적을 찾는다, 단 자주찾지는 않는다. 
    void UpdateTarget()
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
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
            isEnemyInRange = true;
        }
        else
        {
            isEnemyInRange = false;
            target = null;
        }
    }

    private void Update()
    {
        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }

            if (gameObject.tag == "CannonTower" && dangerZone.activeSelf == true)
            {
                dangerZone.SetActive(false);
                Debug.Log("false 들어옴");
            }
            return;
        }

        // 적이 공격범위에 들어왔을때 도형을 생성한다.
        if (gameObject.tag == "CannonTower" && isEnemyInRange == true)
        {
            dangerZone.transform.position = target.transform.position;
            dangerZone.SetActive(true);
            Debug.Log("ture 들어옴");
        }

        // 타겟을 찾는다.
        LockOnTarget();

        // 레이저를 사용할 때
        if (useLaser)
        {
            Laser();
        }

        // 총알을 사용할 때
        else
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    // ★ 단일 레이저 발사
    void Laser()
    {
        // 데미지 적용 (시간에 비례해서)
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);

        // 슬로우 효과 적용
        targetEnemy.Slow(slowAmount);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        // 처음 발사 위치
        lineRenderer.SetPosition(0, firePoint.position);

        // 마지막 위치
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }


    // ★ 총알 / 미사일 발사
    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }

        Debug.Log("SHOOT!");
    }

    // 범위 그리기
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
