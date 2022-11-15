using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlackholeProjectiles : Projectiles
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    float elapsedTime = 0f;
    float InterpolateValue = 1f;
    float maxHeight = 16f;
    float minHeight = 1f;
    private void Update()
    {
        if (target == null)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        elapsedTime += Time.deltaTime;

        // ����� ���
        //if (elapsedTime <= 0.3f)
        //{
        //    transform.Translate(new Vector3(dir.x, maxHeight, dir.z).normalized * distanceThisFrame, Space.World);
        //}
        //else
        //{
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        //}

        transform.LookAt(target);

        if (dir.magnitude <= distanceThisFrame + InterpolateValue || transform.position.y <= InterpolateValue)
        {
            BlackholeExplosion blackholeExplosion = ImpactEffect.GetComponent<BlackholeExplosion>();
            blackholeExplosion.enemyTag = enemyTag;
            blackholeExplosion.damage = damage;

            if (photonView.IsMine)
            {
                PhotonNetwork.Instantiate(ImpactEffect.name, new Vector3(transform.position.x, minHeight, transform.position.z), Quaternion.identity);
            }

            PhotonNetwork.Destroy(gameObject);
            return;
        }
    }
}
