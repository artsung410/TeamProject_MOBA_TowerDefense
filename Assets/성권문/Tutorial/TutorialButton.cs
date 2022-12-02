using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class TutorialButton : MonoBehaviour
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    private Image image;
    public int id;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void onClick()
    {
        TutorialManager.Instance.setDefaultTutorialButtons();
        image.sprite = TutorialManager.Instance.tutorialButtonBackGroundSprites[1];
        TutorialManager.Instance.playVideo(id);
    }
}
