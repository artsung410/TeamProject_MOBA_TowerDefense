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
    public string EnemyTag;
    BoxCollider boxColider;
    private void Awake()
    {
        boxColider = GetComponent<BoxCollider>();   
    }
    private void OnEnable()
    {
        boxColider.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(EnemyTag))
        {
            Debug.Log("��������°ǰ�?");
            if(other.gameObject.layer == 8) // �̴Ͼ�
            {
                other.gameObject.GetComponent<Enemybase>().TakeDamage(PistonDamage);
            }
            if (other.gameObject.layer == 7) // �÷��̾�
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
