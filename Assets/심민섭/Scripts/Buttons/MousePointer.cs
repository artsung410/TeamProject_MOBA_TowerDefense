using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private void OnEnable()
    {
        transform.localScale = new Vector3(1f, 1f, 0f);
    }

    public IEnumerator MoveMouseCursorPoint()
    {
        while (transform.localScale.x >= 0)
        {
            transform.localScale -= new Vector3(0.01f, 0.01f, 0f);

            if (transform.localScale.x <= 0)
            {

                gameObject.SetActive(false);
            }
            yield return null;
        }
    }
}
