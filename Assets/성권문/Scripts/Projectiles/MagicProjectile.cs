using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
    
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class MagicProjectile : Projectiles, ISeek
{
    public Transform target;
    public float speed = 70f;
    public float explosionRadius = 0f;

    public void Seek(float dmg, Transform tg)
    {
        target = tg;
        damage = dmg;
    }

    float elapsedTime = 0f;
    float InterpolateValue = 1f;
    float maxHeight = 16f;
    float minHeight = 1f;
    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        elapsedTime += Time.deltaTime;

        if (elapsedTime <= 0.3f)
        {
            transform.Translate(new Vector3(dir.x, maxHeight, dir.z).normalized * distanceThisFrame, Space.World);
        }
        else
        {
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }

        transform.LookAt(target);

        if (dir.magnitude <= distanceThisFrame + InterpolateValue || transform.position.y <= InterpolateValue)
        {
            MagicExplosion magicExplosion = ImpactEffect.GetComponent<MagicExplosion>();
            magicExplosion.enemyTag = enemyTag;
            magicExplosion.damage = damage;

            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(ImpactEffect.name, new Vector3(transform.position.x, minHeight, transform.position.z), Quaternion.identity);
            }

            Destroy(gameObject);
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
