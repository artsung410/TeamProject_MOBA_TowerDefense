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
    public static event Action<GameObject, GameObject, string> minionTowerEvent = delegate { };
    public GameObject smokeParticles;

    [Header("�̴Ͼ� Ÿ�� �Ӽ�")]
    public MinionBlueprint MinionDB;
    public GameObject bluePF;
    public GameObject redPF;

    private void Start()
    {
        // Ÿ���� ���÷� ã�����ְ� invoke�� �Ѵ�.
        InvokeRepeating("UpdateTarget", 0f, 0.1f);

        if(!photonView.IsMine)
        {
            return;
        }

        // �̴Ͼ� ����
        if (bluePF != null && redPF != null)
        {
            minionTowerEvent.Invoke(bluePF, redPF, gameObject.tag);
        }

        // ����ũ ȿ�� ����
        GameObject particle = PhotonNetwork.Instantiate(smokeParticles.name, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
        particle.transform.Rotate(new Vector3(0, -90f, 0));
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