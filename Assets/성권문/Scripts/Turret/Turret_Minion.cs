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
        // Ÿ���� ���÷� ã�����ְ� invoke�� �Ѵ�.
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
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
        GameObject circleAttack = PhotonNetwork.Instantiate(towerData.Projectiles.name, partToRotate.transform.position, partToRotate.transform.rotation);

        if (!photonView.IsMine)
        {
            Collider circleCol = circleAttack.GetComponent<Collider>();
            circleCol.enabled = false;
        }

    }
}