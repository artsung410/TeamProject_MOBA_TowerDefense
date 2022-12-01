using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Turret_Magic : Turret
{
    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

        if (!photonView.IsMine)
        {
            return;
        }

        if (effectParticles == null)
        {
            return;
        }

        // 이펙트 파티클 생성
        InitEffectParticles(firePoint.position);
    }

    private void FixedUpdate()
    {
        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            return;
        }

        // 타겟을 찾는다.
        LockOnTarget();


        if (fireCountdown <= 0f)
        {
            Shoot();
            //audioSource.clip = TowerDB.AudioClip_Attack_Name;
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

        makeMagicProjectile();
    }

    void makeMagicProjectile()
    {
        GameObject bulletGO = PhotonNetwork.Instantiate(projectilePF.name, firePoint.position, firePoint.rotation);
        Projectiles projectile = bulletGO.GetComponent<Projectiles>();

        if (projectile != null)
        {
            projectile.speed = projectiles_Speed;
            projectile.enemyTag = enemyTag;
            projectile.EffectID = towerDB.buffID;

            if (towerDB.buffID != 0)
            {
                projectile.EffectID = towerDB.buffID;
            }

            projectile.Seek(attack, target);
        }
    }

    // 범위 그리기
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
