using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################
public class Turret_Arrow : Turret
{
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        //Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
        if (target == null)
        {
            return;
        }

        // Ÿ���� ã�´�.
        LockOnTarget();


        // ���� �ֱ�� �Ѿ� �߻�
        if (fireCountdown <= 0f)
        {
            Shoot();
            audioSource.clip = towerData.SoundAttack;
            audioSource.Play();
            fireCountdown = 1f / attackSpeed;
        }
        fireCountdown -= Time.deltaTime;
    }

    protected override void Shoot()
    {
        GameObject bulletGO = PhotonNetwork.Instantiate(towerData.Projectiles.name, firePoint.position, firePoint.rotation);

        if (!photonView.IsMine)
        {
            Collider bulletCol = bulletGO.GetComponent<Collider>();
            bulletCol.enabled = false;
        }

        Arrow arrow = bulletGO.GetComponent<Arrow>();

        if (arrow != null)
        {
            arrow.enemyTag = enemyTag;
            arrow.Seek(attack, target);
        }
    }
}
