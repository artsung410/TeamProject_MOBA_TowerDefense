using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Bullet : Projectiles, ISeek
{
    public float explosionRadius = 0f;

    float InterpolateValue = 1f;
    private void Update()
    {
        if (target == null)
        {
            if(photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        transform.LookAt(target);

        if (dir.magnitude <= distanceThisFrame || transform.position.y <= InterpolateValue)
        {
            HitTarget();
            return;
        }
    }

    void HitTarget()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Instantiate(ImpactEffect.name, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        }

        Explode();
        PhotonNetwork.Destroy(gameObject);
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