using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class Turret_Buff : MonoBehaviour
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

    [Header("====== ����ü ======")]

    [Header("����ü ������")]
    public GameObject SkillPrefab;


    private void Start()
    {
        // Ÿ���� ���÷� ã�����ְ� invoke�� �Ѵ�.
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
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

        }
        else
        {

            target = null;
        }
    }

    private void Update()
    {
        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
        if (target == null)
        {
            return;
        }

        // Ÿ�� �������� ȸ���Ѵ�.
        LockOnTarget();

        // �Ѿ��� ����� ��
        if (fireCountdown <= 0f)
        {
            Fire();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    // �� �Ѿ� / �̻��� �߻�
    void Fire()
    {
        GameObject newSkill = Instantiate(SkillPrefab, target.position, Quaternion.identity);
        Destroy(newSkill, 2f);
    }

    // Ÿ�� �������� ȸ���ϱ�
    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
