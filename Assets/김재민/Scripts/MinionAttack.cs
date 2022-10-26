using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttack : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    [SerializeField]
    float PistonDamage = 5f;
    BoxCollider boxColider;
    EnemySatatus satatus;
    private void Awake()
    {
        satatus = GetComponent<EnemySatatus>();
        boxColider = GetComponent<BoxCollider>();   
    }
    private void OnEnable()
    {
        boxColider.enabled = false;
        if(satatus._eminiomtype == EnemySatatus.EMINIOMTYPE.Special)
        {
            PistonDamage += 15f;
            
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(satatus.EnemyTag) == false)
        {
            return;
        }

        if(other.CompareTag(satatus.EnemyTag))
        {

            Debug.Log("��������°ǰ�?");
            if(other.gameObject.layer == 8 && satatus._eminiomtype == EnemySatatus.EMINIOMTYPE.Nomal) // �̴Ͼ� ����
            {
                other.gameObject.GetComponent<Enemybase>().TakeDamage(PistonDamage);
            }
            if (other.gameObject.layer == 7 &&  satatus._eminiomtype == EnemySatatus.EMINIOMTYPE.Nomal) // �÷��̾� ����
            {
                other.gameObject.GetComponent<Health>().OnDamage(PistonDamage);
            }
            if (other.gameObject.layer == 6) // Ÿ��
            {
                other.gameObject.GetComponent<Turret>().TakeDamage(PistonDamage);
            }
            if (other.gameObject.layer == 12) // �ؼ���
            {
                other.gameObject.GetComponent<NexusHp>().TakeOnDagmage(PistonDamage);
            }
            if (other.gameObject.layer == 13 && satatus._eminiomtype == EnemySatatus.EMINIOMTYPE.Nomal) // Ư���̴Ͼ�
            {
                other.gameObject.GetComponent<Enemybase>().TakeDamage(PistonDamage);
            }
        }
    }

    public void AttackboxOn()
    {
        boxColider.enabled = true; 
    }


    public void Attackboxoff()
    {
        boxColider.enabled = false;
    }
}
