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
    public GameObject smokeParticles;
    public enum ParticleType
    {
        Fire,
        Frozen,
        Blackhole,
    }
    public ParticleType particleType;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

        if (!photonView.IsMine)
        {
            return;
        }

        if (smokeParticles == null)
        {
            return;
        }

        // 스모크 효과 생성
        GameObject particle = PhotonNetwork.Instantiate(smokeParticles.name, firePoint.position, Quaternion.identity);
        particle.transform.Rotate(new Vector3(0, -90f, 0));
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
            audioSource.clip = towerData.SoundAttack;
            audioSource.Play();
            fireCountdown = 1f / attackSpeed;
        }

        fireCountdown -= Time.deltaTime;
    }

    protected override void Shoot()
    {
        GameObject bulletGO = PhotonNetwork.Instantiate(towerData.Projectiles.name, firePoint.position, firePoint.rotation);

        if (particleType == ParticleType.Fire)
        {
            FireProjectile fire = bulletGO.GetComponent<FireProjectile>();

            if (fire != null)
            {
                fire.enemyTag = enemyTag;
                fire.Seek(attack, target);
            }
        }

        else if (particleType == ParticleType.Frozen)
        {
            FrozenProjectile frozen = bulletGO.GetComponent<FrozenProjectile>();

            if (frozen != null)
            {
                frozen.enemyTag = enemyTag;
                frozen.Seek(attack, target);
            }
        }
    }

    // 범위 그리기
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerData.AttackRange);
    }
}
