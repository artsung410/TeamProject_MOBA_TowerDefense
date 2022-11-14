using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turret_Cannon : Turret
{
    public GameObject smokeParticles;

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
        GameObject bulletGO = PhotonNetwork.Instantiate(towerData.Projectiles.name, firePoint.position, firePoint.rotation);

        if (!photonView.IsMine)
        {
            Collider bulletCol = bulletGO.GetComponent<Collider>();
            bulletCol.enabled = false;
        }

        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            // 투사체의 공격력을 적용하고, 타겟을 넘겨줌.
            bullet.enemyTag = enemyTag;
            bullet.Seek(attack, shotTransform);
        }
    }

    IEnumerator delayShoot()
    {
        yield return new WaitForSeconds(1f);
        Shoot();
        audioSource.clip = towerData.SoundAttack;
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        isLockOn = false;
    }

    void makeDangerZone()
    {
        NewDangerZone = Instantiate(DangerZonePF, firePoint.position, firePoint.rotation);
        NewDangerZone.transform.position = new Vector3(target.position.x, 0.5f, target.position.z);
        shotTransform = NewDangerZone.transform;
    }

    // dangerZone기준으로 설정
    void LockOnTarget_dangerZone()
    {
        Vector3 dir = shotTransform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    // 범위 그리기
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerData.AttackRange);
    }
}
