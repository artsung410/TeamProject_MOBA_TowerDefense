using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    //public Vector3 target;
    //public bool isTargetOn;

    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }

    //private void Update()
    //{
    //    if (isTargetOn == false)
    //    {
    //        return;
    //    }

    //    transform.position = new Vector3(target.x, target.y, target.z);
    //}
}
