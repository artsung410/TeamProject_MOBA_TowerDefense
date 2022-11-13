using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Turret_Magic : Turret
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

        // ����ũ ȿ�� ����
        GameObject particle = PhotonNetwork.Instantiate(smokeParticles.name, firePoint.position, Quaternion.identity);
        particle.transform.Rotate(new Vector3(0, -90f, 0));
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
        GameObject bulletGO = PhotonNetwork.Instantiate(towerData.Projectiles.name, firePoint.position, firePoint.rotation);
        MagicProjectile magic = bulletGO.GetComponent<MagicProjectile>();

        if (magic != null)
        {
            magic.enemyTag = enemyTag;
            magic.Seek(attack, shotTransform);
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

    // dangerZone�������� ����
    void LockOnTarget_dangerZone()
    {
        Vector3 dir = shotTransform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    // ���� �׸���
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerData.AttackRange);
    }
}
