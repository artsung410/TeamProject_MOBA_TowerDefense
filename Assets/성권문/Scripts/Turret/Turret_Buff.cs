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
        // Ÿ���� ���÷� ã�����ְ� invoke�� �Ѵ�.
        InvokeRepeating("UpdateTarget", 0f, 0.1f);

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

    // ���� ����� ���� ã�´�, �� ����ã���� �ʴ´�. 
    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
        if (target == null)
        {
            return;
        }

        // Ÿ�� �������� ȸ���Ѵ�.
        LockOnTarget();

        // �Ѿ��� ����� ��
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