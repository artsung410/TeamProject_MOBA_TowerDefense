using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Laser : Turret
{
    [Header("������ Ÿ�� �Ӽ�")]
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

        // ���� ���������� ����� target�� null�� �Ǹ� �����Ѵ�.
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

        // Ÿ���� ã�´�.
        LockOnTarget();
        Laser();

    }

    // �� ���� ������ �߻�
    void Laser()
    {
        // ������ ���� (�ð��� ����ؼ�)
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);

        // ���ο� ȿ�� ����
        //targetEnemy.Slow(slowAmount);
        //if (!lineRenderer.enabled)
        //{
        //    lineRenderer.enabled = true;
        //    impactEffect.Play();
        //    impactLight.enabled = true;
        //}

        // ó�� �߻� ��ġ
        lineRenderer.SetPosition(0, firePoint.position);

        // ������ ��ġ
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }
}
