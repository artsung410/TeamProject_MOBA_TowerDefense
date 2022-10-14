using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;

    [Header("��Ÿ�")]
    public float range = 15f;

    [Header("�����ֱ�(�ʴ� n�� �߻�)")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Ÿ�� TAG")]
    public string enemyTag = "Enemy";

    [Header("ȸ��ü")]
    public Transform partToRotate;

    [Header("ȸ���ӵ�")]
    public float turnSpeed = 10f;

    [Header("���ݹ��� ����")]
    public GameObject dangerZonePrefab;
    private GameObject dangerZone;
    Vector3 dangerZonePrevPos;
    private bool isEnemyInRange;

    [Header("====== ����ü ======")]

    [Header("����ü ������")]
    public GameObject bulletPrefab;

    [Header("����ü �߻� ��ġ")]
    public Transform firePoint;

    [Header("====== ���� ������ �Ӽ�======")]
    public bool useLaser = false;
    public int damageOverTime = 30;
    public float slowAmount = 0.5f;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    private void Start()
    {

        // �̻��� �ͷ��϶��� dangerZone�������� 
        if (gameObject.tag == "CannonTower")
        {
            dangerZone = Instantiate(dangerZonePrefab, transform.position, transform.rotation);
            dangerZone.SetActive(false);
        }

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // ���� ����� ���� ã�´�, �� ����ã���� �ʴ´�. 
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
        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
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
                Debug.Log("false ����");
            }
            return;
        }

        // ���� ���ݹ����� �������� ������ �����Ѵ�.
        if (gameObject.tag == "CannonTower" && isEnemyInRange == true)
        {
            dangerZone.transform.position = target.transform.position;
            dangerZone.SetActive(true);
            Debug.Log("ture ����");
        }

        // Ÿ���� ã�´�.
        LockOnTarget();

        // �������� ����� ��
        if (useLaser)
        {
            Laser();
        }

        // �Ѿ��� ����� ��
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

    // �� ���� ������ �߻�
    void Laser()
    {
        // ������ ���� (�ð��� ����ؼ�)
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);

        // ���ο� ȿ�� ����
        targetEnemy.Slow(slowAmount);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        // ó�� �߻� ��ġ
        lineRenderer.SetPosition(0, firePoint.position);

        // ������ ��ġ
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }


    // �� �Ѿ� / �̻��� �߻�
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

    // ���� �׸���
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
