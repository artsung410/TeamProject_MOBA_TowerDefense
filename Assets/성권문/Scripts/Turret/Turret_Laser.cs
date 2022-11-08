using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Laser : Turret
{
    [Header("레이저 타워 속성")]
    public int damageOverTime = 30;
    public float slowAmount = 0.5f;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
            }
            return;
        }

        // 타겟을 찾는다.
        LockOnTarget();
        Laser();

    }

    // ★ 단일 레이저 발사
    void Laser()
    {
        // 데미지 적용 (시간에 비례해서)
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);

        // 슬로우 효과 적용
        //targetEnemy.Slow(slowAmount);
        //if (!lineRenderer.enabled)
        //{
        //    lineRenderer.enabled = true;
        //    impactEffect.Play();
        //    impactLight.enabled = true;
        //}

        // 처음 발사 위치
        lineRenderer.SetPosition(0, firePoint.position);

        // 마지막 위치
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }
}
