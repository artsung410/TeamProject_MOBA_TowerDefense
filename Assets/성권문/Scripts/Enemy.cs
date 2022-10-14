using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("초기 이동속도")]
    public float startSpeed = 10f;

    [HideInInspector]
    public float speed;

    [Header("체력")]
    public float health = 100;

    [Header("처치시 골드")]
    public int worth = 50;

    [Header("사망 효과")]
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

    // 적이 죽으면 일정량의 골드를 획득;
    void Die()
    {
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        PlayerStats.Money += worth;
        Destroy(gameObject);
    }
}