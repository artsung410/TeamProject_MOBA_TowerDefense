using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turret_Cannon : Turret
{
    private Transform target;
    private EnemyMinion targetEnemy;

    [Header("��Ÿ�")]
    public float range = 15f;

    [Header("�����ֱ�(�ʴ� n�� �߻�)")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("ȸ��ü")]
    public Transform partToRotate;

    [Header("ȸ���ӵ�")]
    public float turnSpeed = 10f;

    [Header("���ݹ��� ����")]
    public GameObject dangerZonePrefab;
    private GameObject dangerZone;
    private bool isEnemyInRange;

    [Header("====== ����ü ======")]

    [Header("����ü ������")]
    public GameObject bulletPrefab;

    [Header("����ü �߻� ��ġ")]
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

        // ���� ����� ������ �Ÿ�
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

        // ���� �����ȿ� ���԰�, ������ �Ÿ��� ���������� �������
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<EnemyMinion>();
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
        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
        if (target == null)
        {
            if (dangerZone.activeSelf == true)
            {
                dangerZone.SetActive(false);
            }
            return;
        }

        // ���� ���ݹ����� �������� ������ �����Ѵ�.
        if (isEnemyInRange == true)
        {
            dangerZone.transform.position = target.transform.position;
            dangerZone.SetActive(true);
        }

        // Ÿ���� ã�´�.
        LockOnTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    // �� �Ѿ� / �̻��� �߻�
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

    // ���� �׸���
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
