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
    [SerializeField]
    private GameObject closeObj;

    // 카드 뽑기 함수 실행 (드로우 매니저)
    public void BoxOpen()
    {
        // 카드 뽑기
        DrawManager.instance.RandomCardDraw();
        // 아이템 소모
        DrawManager.instance.OpenBoxDisCount();

        closeObj.SetActive(false);
    }
}
