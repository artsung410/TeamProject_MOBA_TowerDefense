using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turret_Cannon : Turret
{
    private Transform target;

    [Header("사거리")]
    public float range = 15f;

    [Header("공격주기(초당 n번 발사)")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("회전체")]
    public Transform partToRotate;

    [Header("회전속도")]
    public float turnSpeed = 10f;

    [Header("공격범위 도형")]
    public GameObject dangerZonePrefab;
    private GameObject dangerZone;
    private bool isEnemyInRange;

    [Header("====== 투사체 ======")]

    [Header("투사체 프리팹")]
    public GameObject bulletPrefab;

    [Header("투사체 발사 위치")]
    public Transform firePoint;
    private void Start()
    {
        dangerZone = PhotonNetwork.Instantiate(dangerZonePrefab.name, transform.position, transform.rotation);
        dangerZone.SetActive(false);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

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
            isEnemyInRange = true;
        }
        else
        {
            isEnemyInRange = false;
            target = null;
        }
    }

    private void FixedUpdate()
    {
        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            if (dangerZone.activeSelf == true)
            {
                dangerZone.SetActive(false);
            }
            return;
        }

        // 적이 공격범위에 들어왔을때 도형을 생성한다.
        if (isEnemyInRange == true)
        {
            dangerZone.transform.position = new Vector3(target.transform.position.x, target.transform.position.y - 0.3f, target.transform.position.z);
            dangerZone.SetActive(true);
            StartCoroutine(DeActivationDangerZone());
        }

        // 타겟을 찾는다.
        LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private IEnumerator DeActivationDangerZone()
    {
        yield return new WaitForSeconds(1f);
        dangerZone.SetActive(false);
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    // ★ 총알 / 미사일 발사
    void Shoot()
    {
        GameObject bulletGO = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);

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
