using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerDownHandler
{
    public Image _chr;
    public Text _cardName;
    public int _cardIndex;

    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // 카드의 정보를 초기화
    public void CardUISet(int cardIndex, Sprite cardImage, string cardName)
    {
        _chr.sprite = cardImage;
        _cardName.text = cardName;
    }
    // 카드가 클릭되면 뒤집는 애니메이션 재생
    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetTrigger("Flip");
    }
}
