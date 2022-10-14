using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class EnemeyMinion : MonoBehaviour
{
    [SerializeField]
    Transform tg;
    NavMeshAgent NavMeshAgent;
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
}
