using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDrawBox : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 드로우버튼을 누르면 카드가 뽑히는데 일단은 아이템 소모부터 구현할꼬임

    // 드로우 박스 닫기
    [SerializeField]
    private GameObject closeObj;
    // 뽑기 창 띄우기
    [SerializeField]
    private GameObject openObj;

    [SerializeField]
    private RandomSelect randomSelect;

    // 카드 뽑기 함수 실행 (드로우 매니저)
    public void BoxOpen()
    {
        // 아이템 소모
        DrawManager.instance.OpenBoxDisCount();

        // 카드 뽑기
        randomSelect.SkillResultSelect();

        closeObj.SetActive(false);
        openObj.SetActive(true);
    }

    public void RetryButton()
    {
        // 아이템 소모
        DrawManager.instance.OpenBoxDisCount();
        // 카드 뽑기 창 초기화
        DrawManager.instance.CardDataInit();
        // 카드 뽑기 실행
        randomSelect.SkillResultSelect();
    }
}
