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

    private void Update()
    {
        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
        if (target == null)
        {
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

    // ���� �׸���
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
