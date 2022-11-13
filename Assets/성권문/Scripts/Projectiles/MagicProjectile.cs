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

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            MagicExplosion magicExplosion = ImpactEffect.GetComponent<MagicExplosion>();
            magicExplosion.enemyTag = enemyTag;
            magicExplosion.damage = damage;

            GameObject magicExplosionPF = PhotonNetwork.Instantiate(ImpactEffect.name, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);

            Destroy(magicExplosionPF, 3f);
            Destroy(gameObject);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
