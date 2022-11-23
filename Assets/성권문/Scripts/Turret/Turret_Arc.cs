using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################
public class Turret_Arc : Turret
{
    private Animator Arc_animator;

    private void Start()
    {
        Arc_animator = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        StartCoroutine(Arc());
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
    IEnumerator Arc()
    {
        while (true)
        {
            if (target != null)
            {
                Vector3 dir = target.position - transform.position;
                Vector3 dir_y = new Vector3(dir.x, 0f, dir.z);
                partToRotate.transform.rotation = Quaternion.LookRotation(dir_y);

                if (Arc_animator.GetBool("onAttack") == false)
                {
                    Arc_animator.SetBool("onAttack", true);
                }
                else
                {
                    Arc_animator.SetBool("onAttack", false);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // ���� �׸���
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
