using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turret_Laser : Turret
{
    [HideInInspector]
    public float damageOverTime;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;

    public List<Material> ElectricMaterials;

    public GameObject smokeParticles;
    private void Start()
    {
        damageOverTime = attack;
        lineRenderer.enabled = false;
        // TODO : nameof로 바꿀것.
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

        if (!photonView.IsMine)
        {
            return;
        }

        InitEffectParticles(firePoint.position);
    }

    float elapsedTime = 0f;
    private void FixedUpdate()
    {
        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            if (lineRenderer.enabled)
            {
                elapsedTime = 0f;
                lineRenderer.enabled = false;
                impactEffect.Stop();
            }
            return;
        }

        // 타겟을 찾는다.
        LockOnTarget();

        // 레이저를 그린다.
        DrawerLineRenderer();

        if (!photonView.IsMine)
        {
            return;
        }

        // 레이저 데미지를 적용한다.
        Laser();

    }

    void Laser()
    {
        // 데미지 적용 (시간에 비례해서)
        Damage_Laser();
        Vector3 dir = firePoint.position - target.position;
        impactEffect.transform.position = target.position + dir.normalized;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    int i = 0;
    void DrawerLineRenderer()
    {
        elapsedTime += Time.deltaTime;
        List<Material> myMaterial = ElectricMaterials;

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
        }

        // 번개효과
        if (elapsedTime >= 0.02f)
        {
            lineRenderer.material = myMaterial[i];
            i++;
            if (i >= 4)
            {
                i = 0;
            }
            elapsedTime = 0f;
        }

        // 처음 발사 위치
        lineRenderer.SetPosition(0, firePoint.position);

        // 마지막 위치
        lineRenderer.SetPosition(1, target.position);
    }

    void Damage_Laser()
    {
        // 플레이어 데미지 적용
        if (target.gameObject.layer == 7)
        {
            Health player = target.GetComponent<Health>();

            if (player != null && player.gameObject.activeSelf)
            {
                player.OnDamage(damageOverTime);
            }
            else
            {
                return;
            }
        }

        // 미니언 데미지 적용
        else if (target.gameObject.layer == 8)
        {
            Enemybase minion = target.GetComponent<Enemybase>();

            if (minion != null)
            {
                minion.TakeDamage(damageOverTime);
            }
        }

        // 스페셜 미니언 데미지 적용
        else if (target.gameObject.layer == 13)
        {
            Enemybase special_minion = target.GetComponent<Enemybase>();

            if (special_minion != null)
            {
                special_minion.TakeDamage(damageOverTime);
            }
        }
    }
}
