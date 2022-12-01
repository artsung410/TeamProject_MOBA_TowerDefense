using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turret_Cannon : Turret
{
    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    bool isLockOn;
    private void FixedUpdate()
    {
        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
        if (target == null)
        {
            return;
        }

        if (isLockOn == false)
        {
            // Ÿ���� ã�´�.
            LockOnTarget();
        }

        if (fireCountdown <= 0f)
        {
            isLockOn = true;
            makeDangerZone(); // ���� ������� ����
            LockOnTarget_dangerZone();
            StartCoroutine(delayShoot()); // ���� �߻�.
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

        GameObject bulletGO = PhotonNetwork.Instantiate(projectilePF.name, firePoint.position, firePoint.rotation);

        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            // ����ü�� ���ݷ��� �����ϰ�, Ÿ���� �Ѱ���.
            bullet.speed = projectiles_Speed;
            bullet.enemyTag = enemyTag;
            bullet.Seek(attack, shotTransform);
        }
    }

    IEnumerator delayShoot()
    {
        yield return new WaitForSeconds(1f);
        Shoot();
        //audioSource.clip = towerData.SoundAttack;
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        isLockOn = false;
    }

    void makeDangerZone()
    {
        if(photonView.IsMine)
        {
            NewDangerZone = PhotonNetwork.Instantiate(DangerZonePF.name, new Vector3(target.position.x, target.position.y + 0.5f, target.position.z), Quaternion.identity);
            shotTransform = NewDangerZone.transform;
        }
    }

    // dangerZone�������� ����
    void LockOnTarget_dangerZone()
    {
        if (photonView.IsMine)
        {
            Vector3 dir = shotTransform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    // ���� �׸���
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
