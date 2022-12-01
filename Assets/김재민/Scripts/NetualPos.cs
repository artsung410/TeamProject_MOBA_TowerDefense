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
    float attackArea = 20f;
    private void Awake()
    {
        pos = new Vector3(87.775f,2.3f,75.779f) * 2; // ���߾� ������ 
        
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
        float distacne = Vector3.SqrMagnitude(pos - transform.GetChild(0).GetComponent<OrcFSM>().transform.position); // �Ÿ� ���ؼ�
        if(distacne >= attackArea * attackArea) // ���� �������� ũ�ų� ������ ���ͷ� ������
        {
            transform.GetChild(0).GetComponent<OrcFSM>().transform.position = pos;
        }
        
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
                transform.GetChild(0).GetComponent<OrcFSM>().transform.position = pos;
                transform.GetChild(0).GetComponent<OrcFSM>().StateChange();
                
                break;



            }
            yield return null;  
        }

    }
}
