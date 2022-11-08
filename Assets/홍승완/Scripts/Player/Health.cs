using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################
    
    public Slider hpSlider3D;
    Stats _stats;
    PlayerAnimation ani;

    public float health;

    private float _maxHealth;
    private float _prevMaxHealth;
    public bool isDeath
    {
        get;
        private set;
    }

    private void Awake()
    {
        _stats = GetComponent<Stats>();
        ani = GetComponent<PlayerAnimation>();
    }

    private void OnEnable()
    {

    }

    private void Start()
    {
        isDeath = false;
        _maxHealth = _stats.MaxHealth;
        _prevMaxHealth = _maxHealth;

        // �ʱ�ȭ �κ��� �ִ�ü������
        health = _maxHealth;

        hpSlider3D.maxValue = _maxHealth;
        hpSlider3D.value = health;
    }


    [PunRPC]
    public void HealthUpdate(float maxHP)
    {
        _maxHealth = maxHP;

        // ���� ���� ü�¿��� ������ ü�·� ��ŭ ���� ä���ش�
        health += (_maxHealth - _prevMaxHealth);
        hpSlider3D.value = health;

        // �ٽ� preveMaxHealth�� �ֽ�ȭ�Ѵ�
        _prevMaxHealth = _maxHealth;

        hpSlider3D.maxValue = _maxHealth;
    }

    // �������� ������ hp�� ���ҵǴ� �޼���
    [PunRPC]
    public void ReduceHealthPoint(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        hpSlider3D.value = health;
        Die();
    }

    //[PunRPC]
    public void OnDamage(float damage)
    {
        if (isDeath)
        {
            StopCoroutine(DelayDisapearBody());
            return;
        }

        photonView.RPC(nameof(ReduceHealthPoint), RpcTarget.All, damage);

    }

    public void Die()
    {
        if (health <= 0f)
        {
            PlayerHUD.Instance.AddScoreToEnemy(gameObject.tag);
            isDeath = true;
            ani.DieMotion();
            hpSlider3D.gameObject.SetActive(false);

            StartCoroutine(DelayDisapearBody());
        }
    }

    IEnumerator DelayDisapearBody()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

}
