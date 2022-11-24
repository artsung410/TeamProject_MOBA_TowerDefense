using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerDownHandler
{
    public Image _frontImage;
    public Image _backImage;
    public Text _cardName;
    public Text _cardDesc;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // 카드의 정보를 초기화
    public void CardUISet(Sprite frontCardImage, Sprite backCardImage, string cardName, string cardDesc)
    {
        _frontImage.sprite = frontCardImage;
        _backImage.sprite = backCardImage;
        _cardName.text = cardName;
        _cardDesc.text = cardDesc;
    }
    // 카드가 클릭되면 뒤집는 애니메이션 재생
    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetTrigger("Flip");
    }

    public void AllOpenAnimation()
    {
        animator.SetTrigger("Flip");
    }
}
