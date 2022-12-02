using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField]
    private GameObject videoCanvas;

    [Header("Ʃ�丮�� ��ư")]
    public GameObject button;

    [Header("Ʃ�丮�� �г�")]
    public GameObject panel;

    [Header("Ʃ�丮�� ��ư")]
    public List<GameObject> tutorialButtons;
    public List<Sprite> tutorialButtonBackGroundSprites;

    [Header("���� �÷��̾�")]
    public VideoPlayer videoPlayer;

    [Header("Ʃ�丮�� ����")]
    public List<VideoClip> videoClips;

    private bool onVideo= false;

    private void Awake()
    {
        Instance = this;
    }

    public void DeActivationPanel()
    {
        panel.SetActive(false);
        button.SetActive(true);
    }

    public void ActivationPanel()
    {
        panel.SetActive(true);
        button.SetActive(false);
        initTutorialButtons();
    }

    public void ActivationTutorialButton()
    {
        button.SetActive(true);
    }

    public void DeActivationTutorialButton()
    {
        button.SetActive(false);
        Debug.Log("���濭��");
    }

    public void ActivationVideo()
    {
        onVideo = !onVideo;
        videoCanvas.SetActive(onVideo);
    }
    
    public void setDefaultTutorialButtons()
    {
        for (int i =0; i < tutorialButtons.Count; i++)
        {
            tutorialButtons[i].GetComponent<Image>().sprite = tutorialButtonBackGroundSprites[0];
        }
    }

    public void initTutorialButtons()
    {
        tutorialButtons[0].GetComponent<TutorialButton>().onClick();
    }

    public void playVideo(int id)
    {
        videoPlayer.clip = videoClips[id];
        videoPlayer.Play();
    }
}
