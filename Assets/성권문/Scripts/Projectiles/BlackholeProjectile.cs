using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlackholeProjectile : Projectiles
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    float elapsedTime = 0f;
    float InterpolateValue = 1f;

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

        if (!photonView.IsMine)
        {
            return;
        }

        if (dir.magnitude <= distanceThisFrame + InterpolateValue || transform.position.y <= InterpolateValue)
        {
            GameObject newBlackhole = PhotonNetwork.Instantiate(ImpactEffect.name, target.position, Quaternion.identity);
            BlackholeExplosion blackholeExplosion = newBlackhole.GetComponent<BlackholeExplosion>();
            blackholeExplosion.damage = damage;
            PhotonNetwork.Destroy(gameObject);
            return;
        }
    }
}
