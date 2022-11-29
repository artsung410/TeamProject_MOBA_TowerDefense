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

        // ����Ʈ ��ƼŬ ����
        InitEffectParticles(firePoint.position);
    }

    private void FixedUpdate()
    {
        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
        if (target == null)
        {
            return;
        }

        // Ÿ���� ã�´�.
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

    // ���� �׸���
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
