using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMonetTimer : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private void OnEnable()
    {
        StartCoroutine(ShowImageTimer());
    }

    IEnumerator ShowImageTimer()
    {
        yield return new WaitForSeconds(6f);
        gameObject.SetActive(false);
    }

}
