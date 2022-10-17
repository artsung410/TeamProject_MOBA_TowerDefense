using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 슬롯에 아이템 저장한다.
public class ItemOnObject : MonoBehaviour
{
    // 아이템 정보 가져오고
    public Item item;
    // 아이템을 가지고 있는 갯수를 출력해줄 텍스트
    private Text text;
    // 아이템을 보여줄 이미지
    private Image image;

    void Update()
    {
        // 아이템의 갯수를 출력해준다.
        text.text = "" + item.itemValue;
        // 아이템의 이미지를 출력해준다.
        image.sprite = item.itemIcon;

        // 아이템의 텍스트랑 이미지만 업데이트를 한다.
        GetComponent<ConsumeItem>().item = item;
    }

    void Start()
    {
        // 하위 0번째 오브젝트의 이미지 컴포넌트를 가져온다.
        image = transform.GetChild(0).GetComponent<Image>();
        // 스프라이트 이미지를 설정한다.
        transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;

        // 하위 1번째 오브젝트의 텍스트 컴포넌트를 가져온다.
        text = transform.GetChild(1).GetComponent<Text>();
    }
}
