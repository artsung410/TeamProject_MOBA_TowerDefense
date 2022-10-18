using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################
   
    
    public Transform tg { get;  set; }

    new Rigidbody rigidbody;
    public float turn;
    public float ballVelocity;
    public int Damage { get; set; }
    // Update is called once per frame
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Damage = 10;
    }

    private void FixedUpdate()
    {
        if (tg == null)
        {
            return;
        }
        // 유도탄
        if(tg.position != null) //타켓이 있을때
        {
            
            rigidbody.velocity = transform.forward * ballVelocity;
            var ballTargetRotation = Quaternion.LookRotation(tg.position + new Vector3(0, 0.8f) - transform.position);
            rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, ballTargetRotation, turn));
            
        }
        else 
            Destroy(gameObject);
        
    }

   


}
