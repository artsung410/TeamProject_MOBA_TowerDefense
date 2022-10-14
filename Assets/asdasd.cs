using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class asdasd : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent navMeshAgent;
    [SerializeField]
    Transform tg;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(tg.position);
    }
}
