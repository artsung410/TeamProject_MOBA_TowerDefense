using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turret_Cannon : Turret
{
    private Transform target;

    [Header("ȸ��ü")]
    public Transform partToRotate;

    [Header("ȸ���ӵ�")]
    public float turnSpeed = 10f;

    [Header("���ݹ��� ����")]
    public GameObject dangerZonePrefab;

    [Header("====== ����ü ======")]

    [Header("����ü ������")]
    public GameObject bulletPrefab;

    [Header("����ü �߻� ��ġ")]
    public Transform firePoint;

    public GameObject DangerZonePF;
    private GameObject NewDangerZone;
    private Transform shotTransform;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            float distanceToEnemy = Vector3.Distance(transform.position ,enemy.transform.position);

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
        }
        else
        {
            target = null;
        }
    }

    bool isLockOn;
    private void FixedUpdate()
    {
        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
        if (target == null)
        {
            return;
        }

        if (isLockOn == false)
        {
            // Ÿ���� ã�´�.
            LockOnTarget();
        }

        if (fireCountdown <= 0f)
        {
            isLockOn = true;
            makeDangerZone(); // ���� ������� ����
            LockOnTarget_dangerZone();
            StartCoroutine(delayShoot()); // ���� �߻�.
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    IEnumerator delayShoot()
    {
        yield return new WaitForSeconds(1f);
        Shoot();
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        isLockOn = false;
    }

    void makeDangerZone()
    {
        NewDangerZone = Instantiate(DangerZonePF, firePoint.position, firePoint.rotation);
        NewDangerZone.transform.position = new Vector3(target.position.x, 0.5f, target.position.z);
        shotTransform = NewDangerZone.transform;
    }

    // Ÿ�� �������� ����
    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    // dangerZone�������� ����
    void LockOnTarget_dangerZone()
    {
        Vector3 dir = shotTransform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    // �� �Ѿ� / �̻��� �߻�
    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(shotTransform);
        }

        //Debug.Log("SHOOT!");
    }

    // ���� �׸���
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
