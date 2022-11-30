using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Turret_Minion : Turret
{
    public static event Action<int, GameObject, GameObject, string> minionTowerEvent = delegate { };

    [Header("미니언 타워 속성")]
    public MinionBlueprint MinionBluePrint;
    public GameObject bluePF;
    public GameObject redPF;

    private void Start()
    {
        // 타겟을 수시로 찾을수있게 invoke를 한다.
        InvokeRepeating("UpdateTarget", 0f, 0.1f);

        if (!photonView.IsMine)
        {
            return;
        }

        // 미니언 생성
        if (bluePF != null && redPF != null)
        {
            minionTowerEvent.Invoke(towerDB.MinionID, bluePF, redPF, gameObject.tag);
        }

        // 이펙트 파티클 생성
        Vector3 interpolationPos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        InitEffectParticles(interpolationPos);
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
        GameObject projectiles = PhotonNetwork.Instantiate(projectilePF.name, partToRotate.transform.position, partToRotate.transform.rotation);

        StraightAttack straight = projectiles.GetComponent<StraightAttack>();
        straight.enemyTag = enemyTag;
        straight.damage = attack;

        if (!photonView.IsMine)
        {
            Collider circleCol = projectiles.GetComponent<Collider>();
            circleCol.enabled = false;
        }

    }
}