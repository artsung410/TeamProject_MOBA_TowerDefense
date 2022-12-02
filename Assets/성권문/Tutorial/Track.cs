using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public enum TrackState
{
    play,
    pasue,
    reset,
}
public class Track : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static Track Instance;
    public List<Sprite> playIcon; 
    public VideoPlayer video;
    public GameObject ControlButton;
    Slider tracking;
    bool slide = false;
    bool onPlay = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        tracking = GetComponent<Slider>();

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        slide = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float frame = (float)tracking.value * (float)video.frameCount;
        video.frame = (long)frame;
        slide = false;
    }

    private void Update()
    {
        if (!slide && video.isPlaying)
        {
            tracking.value = (float)video.frame / (float)video.frameCount;
        }
    }

    public void OnPlay()
    {
        onPlay = !onPlay;

        if (onPlay)
        {
            video.Play();
            ControlButton.GetComponent<Image>().sprite = playIcon[(int)TrackState.pasue];
        }
        else if (!onPlay)
        {
            video.Pause();
            ControlButton.GetComponent<Image>().sprite = playIcon[(int)TrackState.play];
        }
    }
}
