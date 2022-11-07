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

    [Header("====== 투사체 ======")]

    [Header("투사체 발사 위치")]
    public Transform firePoint;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            return;
        }

        // 타겟을 찾는다.
        LockOnTarget();


        // 일정 주기로 총알 발사
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

    // ★ 총알 / 미사일 발사
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
