using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public float waitTime = 10f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene("Lobby");
    }
}
