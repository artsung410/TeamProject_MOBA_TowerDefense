using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class EnemyMinion : MonoBehaviour
{
    [SerializeField]
    Transform tg;
    NavMeshAgent NavMeshAgent;
    public float Hp;

    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        NavMeshAgent.SetDestination(tg.position);
    }
    
    public void TakeDamage(float Damage)
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
            return;
        }

        Hp -= Damage;
    }
}
