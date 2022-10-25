using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
    [SerializeField]
    float SpecialDamage = 20f;
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
        if (other.CompareTag(EnemyTag))
        {

            if (other.gameObject.layer == 6) // Ÿ��
            {
                other.gameObject.GetComponent<Turret>().TakeDamage(SpecialDamage);
            }
            if (other.gameObject.layer == 12) // �ؼ���
            {
                other.gameObject.GetComponent<NexusHp>().TakeOnDagmage(SpecialDamage);
            }
        }

    }
    public void SecialAttackboxOn()
    {
        boxColider.enabled = true;
    }


    public void Attackboxoff()
    {
        boxColider.enabled = false;
    }
}
