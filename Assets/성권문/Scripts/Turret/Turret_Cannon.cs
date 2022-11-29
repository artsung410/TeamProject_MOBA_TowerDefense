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
        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            return;
        }

        if (isLockOn == false)
        {
            // 타겟을 찾는다.
            LockOnTarget();
        }

        if (fireCountdown <= 0f)
        {
            isLockOn = true;
            makeDangerZone(); // 대포 위험범위 생성
            LockOnTarget_dangerZone();
            StartCoroutine(delayShoot()); // 대포 발사.
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
            // 투사체의 공격력을 적용하고, 타겟을 넘겨줌.
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

    // dangerZone기준으로 설정
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

    // 범위 그리기
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
