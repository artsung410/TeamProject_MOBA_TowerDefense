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
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        //Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            return;
        }

        // 타겟을 찾는다.
        LockOnTarget();


        // 일정 주기로 총알 발사
        if (fireCountdown <= 0f)
        {
            Shoot();
            //audioSource.clip = towerData.SoundAttack;
            audioSource.Play();
            fireCountdown = 1f / attackSpeed;
        }
        fireCountdown -= Time.deltaTime;
    }

    protected override void Shoot()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        GameObject bulletGO = PhotonNetwork.Instantiate(projectilePF.name, firePoint.position, firePoint.rotation);

        Projectiles projectile = bulletGO.GetComponent<Projectiles>();

        if (projectile != null)
        {
            projectile.speed = projectiles_Speed;
            projectile.enemyTag = enemyTag;
            projectile.Seek(attack, target);
        }
    }
}
