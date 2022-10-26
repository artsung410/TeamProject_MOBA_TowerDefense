using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Arrow : Projectiles
{
    private Transform target;

    public float speed = 70f;

    public float explosionRadius = 0f;
    public GameObject ImpactEffect;

    public void Seek(Transform _target)
    {
        Debug.Log("Arrow Seek�ڡڡڡڡڡڡڡڡڡڡڡڡڡڡ�");
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position; 
        float distanceThisFrame = speed * Time.deltaTime;

        Debug.Log("Arrow Dir�ڡڡڡڡڡڡڡڡڡڡڡڡڡڡ�" + dir);
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        Debug.Log("Arrow Translate�ڡڡڡڡڡڡڡڡڡڡڡڡڡڡ�" + dir);

        transform.LookAt(target);
    }

    void HitTarget()
    {
        Debug.Log("Arrow HitTarget�ڡڡڡڡڡڡڡڡڡڡڡڡڡڡ�");
        GameObject effectIns = (GameObject)Instantiate(ImpactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == enemyTag)
            {
                Damage(collider.transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}