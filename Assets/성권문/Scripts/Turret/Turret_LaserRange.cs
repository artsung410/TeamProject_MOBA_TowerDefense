using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_LaserRange : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;

    [Header("��Ÿ�")]
    public float range = 15f;

    [Header("�����ֱ�(�ʴ� n�� �߻�)")]
    public float fireRate = 1f;

    [Header("Ÿ�� TAG")]
    public string enemyTag = "Enemy";

    [Header("ȸ��ü")]
    public Transform partToRotate;

    [Header("ȸ���ӵ�")]
    public float turnSpeed = 10f;

    [Header("====== ���� ������ �Ӽ�======")]
    private Animator Laser_Range_animator;


    private void Start()
    {
        Laser_Range_animator = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        StartCoroutine(Laser_Range());
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
            if (Laser_Range_animator.GetBool("onAttack"))
            {
                StartCoroutine(DeactivationLaser());
            }

            return;
        }
    }

    // �� ������ ������ �߻�
    IEnumerator Laser_Range()
    {
        while (true)
        {
            if (target != null)
            {
                Vector3 dir = target.position - transform.position;
                Vector3 dir_y = new Vector3(dir.x, 0f, dir.z);
                partToRotate.transform.rotation = Quaternion.LookRotation(dir_y);

                if(Laser_Range_animator.GetBool("onAttack") == false)
                {
                    Laser_Range_animator.SetBool("onAttack", true);
                }

            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator DeactivationLaser()
    {
        yield return new WaitForSeconds(0.3f);
        Laser_Range_animator.SetBool("onAttack", false);
    }

    // ���� �׸���
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
