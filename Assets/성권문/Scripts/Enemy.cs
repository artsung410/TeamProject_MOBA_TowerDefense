using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("�ʱ� �̵��ӵ�")]
    public float startSpeed = 10f;

    [HideInInspector]
    public float speed;

    [Header("ü��")]
    public float health = 100;

    [Header("óġ�� ���")]
    public int worth = 50;

    [Header("��� ȿ��")]
    public GameObject deathEffect;

    private void Start()
    {
        speed = startSpeed;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    // ���� ������ �������� ��带 ȹ��;
    void Die()
    {
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        PlayerStats.Money += worth;
        Destroy(gameObject);
    }
}