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
    private void Start()
    {
        // 타겟을 수시로 찾을수있게 invoke를 한다.
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
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
        GameObject circleAttack = PhotonNetwork.Instantiate(towerData.Projectiles.name, partToRotate.transform.position, partToRotate.transform.rotation);

        if (!photonView.IsMine)
        {
            Collider circleCol = circleAttack.GetComponent<Collider>();
            circleCol.enabled = false;
        }

    }
}