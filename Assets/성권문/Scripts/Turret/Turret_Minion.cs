using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Turret_Minion : Turret
{
    public GameObject smokeParticles;

    private void Start()
    {
        // 타겟을 수시로 찾을수있게 invoke를 한다.
        InvokeRepeating("UpdateTarget", 0f, 0.1f);

        if(!photonView.IsMine)
        {
            return;
        }

        // 스모크 효과 생성
        GameObject particle = PhotonNetwork.Instantiate(smokeParticles.name, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
        particle.transform.Rotate(new Vector3(0, -90f, 0));
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
        GameObject projectiles = PhotonNetwork.Instantiate(towerData.Projectiles.name, partToRotate.transform.position, partToRotate.transform.rotation);

        StraightAttack straight = projectiles.GetComponent<StraightAttack>();
        straight.enemyTag = enemyTag;

        if (!photonView.IsMine)
        {
            Collider circleCol = projectiles.GetComponent<Collider>();
            circleCol.enabled = false;
        }

    }
}