using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Laser : Turret
{
    private Transform target;
    private EnemyMinion targetEnemy;

    [Header("ȸ��ü")]
    public Transform partToRotate;

    [Header("ȸ���ӵ�")]
    public float turnSpeed = 10f;

    [Header("====== ���� ������ �Ӽ�======")]

    public Transform firePoint;
    public int damageOverTime = 30;
    public float slowAmount = 0.5f;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    private void Start()
    {
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
            targetEnemy = nearestEnemy.GetComponent<EnemyMinion>();
        }
        else
        {
            target = null;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
        if (target == null)
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
            }
            return;
        }

        // Ÿ���� ã�´�.
        LockOnTarget();
        Laser();

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
        //targetEnemy.Slow(slowAmount);
        //if (!lineRenderer.enabled)
        //{
        //    lineRenderer.enabled = true;
        //    impactEffect.Play();
        //    impactLight.enabled = true;
        //}

        // ó�� �߻� ��ġ
        lineRenderer.SetPosition(0, firePoint.position);

        // ������ ��ġ
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }
}
