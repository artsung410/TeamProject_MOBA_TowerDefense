using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class LoadScene : MonoBehaviour
{
    public string[] SceneNames;

    void Start()
    {
        viewScenes();
    }

    void viewScenes()
    {
        for (int i = 0; i < SceneNames.Length; i++)
        {
            SceneManager.LoadSceneAsync(SceneNames[i], LoadSceneMode.Additive);
        }
    }
}
