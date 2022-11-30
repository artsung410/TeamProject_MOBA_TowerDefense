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
        // TODO : nameof�� �ٲܰ�.
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
        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
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

        // Ÿ���� ã�´�.
        LockOnTarget();

        // �������� �׸���.
        DrawerLineRenderer();

        if (!photonView.IsMine)
        {
            return;
        }

        // ������ �������� �����Ѵ�.
        Laser();

    }

    void Laser()
    {
        // ������ ���� (�ð��� ����ؼ�)
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

        // ����ȿ��
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

        // ó�� �߻� ��ġ
        lineRenderer.SetPosition(0, firePoint.position);

        // ������ ��ġ
        lineRenderer.SetPosition(1, target.position);
    }

    void Damage_Laser()
    {
        // �÷��̾� ������ ����
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

        // �̴Ͼ� ������ ����
        else if (target.gameObject.layer == 8)
        {
            Enemybase minion = target.GetComponent<Enemybase>();

            if (minion != null)
            {
                minion.TakeDamage(damageOverTime);
            }
        }

        // ����� �̴Ͼ� ������ ����
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
