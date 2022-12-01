using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Turret_Buff : Turret
{
    private void Start()
    {
        // 타겟을 수시로 찾을수있게 invoke를 한다.
        InvokeRepeating("UpdateTarget", 0f, 0.1f);

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

    // 가장 가까운 적을 찾는다, 단 자주찾지는 않는다. 
    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            return;
        }

        // 타겟 방향으로 회전한다.
        LockOnTarget();

        // 총알을 사용할 때
        if (fireCountdown <= 0f)
        {
            Fire();
            fireCountdown = 1f / attackSpeed;
        }

        fireCountdown -= Time.deltaTime;
    }

    protected override void Fire()
    {
        GameObject projectiles = PhotonNetwork.Instantiate(projectilePF.name, transform.position, transform.rotation);
        CircleAttack magicCircle = projectiles.GetComponent<CircleAttack>();
        magicCircle.enemyTag = enemyTag;

        if (!photonView.IsMine)
        {
            Collider circleCol = projectiles.GetComponent<Collider>();
            circleCol.enabled = false;
        }
    }
}