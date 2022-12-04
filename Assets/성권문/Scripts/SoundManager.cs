using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource_BackGround;
    public AudioSource audioSource_Tutorial;
    public AudioClip audioClip_lobby;
    public AudioClip audioClip_tutorial;

    private bool isPaused = false;

    private void Start()
    {
        audioSource_BackGround.clip = audioClip_lobby;
        audioSource_Tutorial.clip = audioClip_tutorial;
        audioSource_BackGround.Play();
    }

    public void PauseAndPlayBGM()
    {
        if (audioSource_BackGround == null)
        {
            return;
        }
        isPaused = !isPaused;

        if (isPaused)
        {
            audioSource_BackGround.Pause();
            audioSource_Tutorial.clip = audioClip_tutorial;
            audioSource_Tutorial.Play();
        }
        else
        {
            audioSource_BackGround.Play();
            audioSource_Tutorial.Stop(); 
        }
    }
}
