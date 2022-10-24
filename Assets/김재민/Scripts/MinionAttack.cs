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
            Debug.Log("여기들어오는건가?");
            if(other.gameObject.layer == 8)
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
