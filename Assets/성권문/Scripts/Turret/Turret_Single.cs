using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################
public class Turret_Single : Turret
{
    private Transform target;

    [Header("====== ����ü ======")]

    [Header("����ü �߻� ��ġ")]
    public Transform firePoint;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        if (nearestEnemy != null && shortestDistance <= towerData.AttackRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void FixedUpdate()
    {
        //Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
        if (target == null)
        {
            return;
        }

        // Ÿ���� ã�´�.
        LockOnTarget();


        // ���� �ֱ�� �Ѿ� �߻�
        if (fireCountdown <= 0f)
        {
            Shoot();
            audioSource.clip = towerData.SoundAttack;
            audioSource.Play();
            fireCountdown = 1f / attackSpeed;
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
        GameObject bulletGO = Instantiate(towerData.Projectiles, firePoint.position, firePoint.rotation);

        Arrow arrow = bulletGO.GetComponent<Arrow>();

        if (arrow != null)
        {
            arrow.Seek(target);
        }
    }
}
