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
        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            return;
        }
    }

    // ★ 광범위 레이저 발사
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

    // 범위 그리기
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
