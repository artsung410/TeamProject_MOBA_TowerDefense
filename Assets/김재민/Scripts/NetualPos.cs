using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class NetualPos : MonoBehaviourPun
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    IEnumerator poschagnge;
    Vector3 pos;
    private void Awake()
    {
        pos = new Vector3(0,0,0);
        
    }
    private void OnEnable()
    {
        
    }

    void Start()
    {

        poschagnge = Spawn();
        
        StartCoroutine(poschagnge);
    }

    
    void Update()
    {
        
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
            float distance = Vector3.Distance(pos , transform.position);
            if (distance <= 0.2f)
            {
                transform.position = Vector3.Lerp(transform.position, pos,1);
                transform.GetChild(0).GetComponent<OrcFSM>().transform.position = new Vector3(0,0,0);
                transform.GetChild(0).GetComponent<OrcFSM>().StateChange();
                
                break;



            }
            yield return null;  
        }

    }
}
