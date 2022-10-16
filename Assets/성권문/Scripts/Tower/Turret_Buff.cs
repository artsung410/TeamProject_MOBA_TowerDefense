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

    [Header("====== 투사체 ======")]

    [Header("투사체 프리팹")]
    public GameObject SkillPrefab;


    private void Start()
    {
        // 타겟을 수시로 찾을수있게 invoke를 한다.
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
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

        }
        else
        {

            target = null;
        }
    }

    private void Update()
    {
        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            return;
        }

        // 타겟 방향으로 회전한다.
        LockOnTarget();

        // 총알을 사용할 때
        if (fireCountdown <= 0f)
        {
            Fire();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    // ★ 총알 / 미사일 발사
    void Fire()
    {
        GameObject newSkill = Instantiate(SkillPrefab, target.position, Quaternion.identity);
        Destroy(newSkill, 2f);
    }

    // 타겟 방향으로 회전하기
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
