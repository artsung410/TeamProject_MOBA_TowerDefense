using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################
public class Turret_LaserRange : Turret
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        StartCoroutine(Laser_Range());
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

    private void Update()
    {
        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            return;
        }
    }

    // ★ 광범위 레이저 발사
    IEnumerator Laser_Range()
    {
        while (true)
        {
            if (target != null)
            {
                Vector3 dir = target.position - transform.position;
                Vector3 dir_y = new Vector3(dir.x, 0f, dir.z);
                partToRotate.transform.rotation = Quaternion.LookRotation(dir_y);

                if(animator.GetBool("onAttack") == false)
                {
                    animator.SetBool("onAttack", true);
                }
                else
                {
                    animator.SetBool("onAttack", false);
                }
            }

            yield return new WaitForSeconds(1f / attackSpeed);
        }
    }

    IEnumerator DeactivationLaser()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("onAttack", false);
    }

    // 범위 그리기
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerData.AttackRange);
    }

}
